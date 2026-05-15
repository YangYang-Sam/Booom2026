using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTUtility.UI
{
    /// <summary>
    /// When the sibling <see cref="Button"/> uses <see cref="Selectable.Transition.ColorTint"/>,
    /// mirrors the same tint onto extra <see cref="Image"/>s.
    /// </summary>
    /// <remarks>
    /// Unity's Color Tint calls <see cref="Graphic.CrossFadeColor"/>, which tweens
    /// <see cref="CanvasRenderer"/> color via <c>SetColor</c> — it does <b>not</b> update
    /// <see cref="Graphic.color"/> (<c>m_Color</c>), so the Inspector can stay at white while
    /// the control still looks tinted. This component reads/writes <b>canvas renderer</b> colors.
    /// </remarks>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class ButtonAdditionalColorTintSync : MonoBehaviour
    {
        public enum SyncMode
        {
            /// <summary>Match the target graphic's effective canvas color each sync.</summary>
            MirrorTargetGraphic = 0,
            /// <summary>
            /// Scale each image's baseline canvas color by the same per-channel ratio as the
            /// target's current vs baseline canvas color (multi-colored layouts).
            /// </summary>
            PreservePerImageBase = 1,
        }

        [SerializeField]
        private Button button;

        [Tooltip("Images that should receive the same Color Tint result as the Button's Target Graphic.")]
        [SerializeField]
        private List<Image> additionalImages = new List<Image>();

        [SerializeField]
        private SyncMode syncMode = SyncMode.MirrorTargetGraphic;

        [Tooltip("When disabled, restore additional images to canvas colors captured on enable.")]
        [SerializeField]
        private bool restoreBaseColorsOnDisable = true;

        private Color _baselineTargetCanvasColor;
        private Color[] _baselineImageCanvasColors;
        private Graphic _targetGraphic;

        private void Awake()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }
        }

        private void OnEnable()
        {
            CacheBaselineCanvasColors();
            if (Application.isPlaying)
            {
                Canvas.willRenderCanvases += SyncAdditionalTint;
            }
        }

        private void OnDisable()
        {
            if (Application.isPlaying)
            {
                Canvas.willRenderCanvases -= SyncAdditionalTint;
            }

            if (restoreBaseColorsOnDisable && additionalImages != null && _baselineImageCanvasColors != null &&
                _baselineImageCanvasColors.Length == additionalImages.Count)
            {
                for (int i = 0; i < additionalImages.Count; i++)
                {
                    var img = additionalImages[i];
                    if (img != null)
                    {
                        SetCanvasColor(img, _baselineImageCanvasColors[i]);
                    }
                }
            }
        }

        private void SyncAdditionalTint()
        {
            if (!Application.isPlaying || !isActiveAndEnabled || button == null || !button.isActiveAndEnabled)
            {
                return;
            }

            if (button.transition != Selectable.Transition.ColorTint)
            {
                return;
            }

            _targetGraphic = button.targetGraphic;
            if (_targetGraphic == null || _targetGraphic.canvasRenderer == null ||
                additionalImages == null || additionalImages.Count == 0)
            {
                return;
            }

            if (_baselineImageCanvasColors == null || _baselineImageCanvasColors.Length != additionalImages.Count)
            {
                CacheBaselineCanvasColors();
            }

            Color targetEffective = _targetGraphic.canvasRenderer.GetColor();

            for (int i = 0; i < additionalImages.Count; i++)
            {
                var img = additionalImages[i];
                if (img == null || img.canvasRenderer == null || img == _targetGraphic)
                {
                    continue;
                }

                switch (syncMode)
                {
                    case SyncMode.MirrorTargetGraphic:
                        SetCanvasColor(img, targetEffective);
                        break;
                    case SyncMode.PreservePerImageBase:
                        SetCanvasColor(
                            img,
                            ScaleByTargetRatio(_baselineImageCanvasColors[i], targetEffective, _baselineTargetCanvasColor));
                        break;
                }
            }
        }

        private void CacheBaselineCanvasColors()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }

            _targetGraphic = button != null ? button.targetGraphic : null;
            _baselineTargetCanvasColor = GetCanvasColor(_targetGraphic);

            if (additionalImages == null || additionalImages.Count == 0)
            {
                _baselineImageCanvasColors = null;
                return;
            }

            _baselineImageCanvasColors = new Color[additionalImages.Count];
            for (int i = 0; i < additionalImages.Count; i++)
            {
                _baselineImageCanvasColors[i] = GetCanvasColor(additionalImages[i]);
            }
        }

        private static Color GetCanvasColor(Graphic g)
        {
            return g != null && g.canvasRenderer != null ? g.canvasRenderer.GetColor() : Color.white;
        }

        private static void SetCanvasColor(Graphic g, Color c)
        {
            if (g != null && g.canvasRenderer != null)
            {
                g.canvasRenderer.SetColor(c);
            }
        }

        private static Color ScaleByTargetRatio(Color imageBase, Color targetCurrent, Color targetBase)
        {
            return new Color(
                imageBase.r * SafeRatio(targetCurrent.r, targetBase.r),
                imageBase.g * SafeRatio(targetCurrent.g, targetBase.g),
                imageBase.b * SafeRatio(targetCurrent.b, targetBase.b),
                imageBase.a * SafeRatio(targetCurrent.a, targetBase.a));
        }

        private static float SafeRatio(float current, float baseline)
        {
            if (baseline <= Mathf.Epsilon)
            {
                return current <= Mathf.Epsilon ? 1f : 0f;
            }

            return current / baseline;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }
        }
#endif
    }
}
