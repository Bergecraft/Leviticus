using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities.Modules.Shields
{
    public class ShieldBehaviour : SpriteBehaviour<ShieldGeneratorDef>
    {
        float _shield = 0;
        public float Shield
        {
            get
            {
                return _shield;
            }
            set
            {
                _shield = Mathf.Clamp(value, 0, def.maxShield);
            }
        }
        public float ShieldPercentage
        {
            get
            {
                return _shield / def.maxShield;
            }
            set
            {
                Shield = def.maxShield * value;
            }
        }

        float radius;
        Sprite splashSprite;
        Color DefaultColor = new Color(0.28f, 0.65f, 1);
        void Start()
        {
            base.Start();
            _shield = def.maxShield;
            radius = CreateShieldTrigger();

            splashSprite = Resources.Load<Sprite>("modules/HardpointsXL_xcf-ShieldGradiant");
            
            var rt = new RenderTexture(64, 64, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Default);
            rt.filterMode = FilterMode.Point;

            var mat = Instantiate(Resources.Load<Material>("modules/MaskMaterial"));
            mat.shader = Instantiate(Resources.Load<Shader>("modules/MaskShader"));
            mat.SetTexture("_AlphaTex", rt);

            CreateHexagonalPattern(mat);

            CreateBubble(mat);

            CreateShieldSplashCamera(rt);
        }

        private float CreateShieldTrigger()
        {
            if (gameObject.GetComponent<CircleCollider2D>() == null)
            {
                var cc2d = gameObject.AddComponent<CircleCollider2D>();
                cc2d.isTrigger = true;
                cc2d.radius = def.defaultRadius;
            }
            return gameObject.GetComponent<CircleCollider2D>().radius;
        }

        SpriteRenderer hexsr;
        private void CreateHexagonalPattern(Material mat)
        {
            var hexgo = new GameObject("Hex");
            hexgo.transform.parent = transform;
            hexgo.transform.localScale = new Vector3(radius, radius, 1);
            hexgo.transform.localPosition = Vector3.zero;
            hexsr = hexgo.AddComponent<SpriteRenderer>();
            hexsr.sprite = Resources.Load<Sprite>("modules/HardpointsXL_xcf-FadedShieldBubbleHex");
            hexsr.color = DefaultColor;
            hexsr.material = mat;
        }

        SpriteRenderer bubblesr;
        private void CreateBubble(Material mat)
        {
            var bubblego = new GameObject("Bubble");
            bubblego.transform.parent = transform;
            bubblego.transform.localScale = new Vector3(radius, radius, 1);
            bubblego.transform.localPosition = Vector3.zero;
            bubblesr = bubblego.AddComponent<SpriteRenderer>();
            bubblesr.sprite = Resources.Load<Sprite>("modules/HardpointsXL_xcf-ShieldBubble");
            bubblesr.color = DefaultColor;
            bubblesr.material = mat;
        }

        private void CreateShieldSplashCamera(RenderTexture rt)
        {
            var camgo = new GameObject("Camera");
            camgo.transform.parent = transform;
            camgo.transform.localPosition = Vector3.forward*-10;
            var cam = camgo.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.clear;
            cam.cullingMask = 1 << LayerMask.NameToLayer("shieldSplash");
            cam.orthographic = true;
            cam.orthographicSize = radius;
            cam.renderingPath = RenderingPath.VertexLit;
            cam.targetTexture = rt;
            cam.useOcclusionCulling = false;
        }
        public float Damage(float damage, Vector3 position)
        {
            if (Shield > 0)
            {
                CreateShieldSplash(position);
            }
            var overflow = -Mathf.Min(Shield - damage, 0);
            Shield -= damage;
            SetShieldColor(Color.Lerp(Color.red*0.3f, DefaultColor, ShieldPercentage));
            return overflow;
        }
        private void SetShieldColor(Color color){
            hexsr.color = color;
            bubblesr.color = color;
        }
        const float SPLASH_SIZE = 0.3f;
        const float SPLASH_FADE_TIME = 1f;

        private void CreateShieldSplash(Vector3 position)
        {
            var splashgo = new GameObject("Splash");
            var splashsr = splashgo.AddComponent<SpriteRenderer>();
            splashgo.layer = 11;
            splashsr.sprite = splashSprite;
            splashgo.transform.localScale = Vector3.zero;
            splashgo.transform.position = position;
            splashgo.transform.parent = transform;
            var seq = DOTween.Sequence();
            seq.Append(DOTween.To(() => splashgo.transform.localScale, (s) => splashgo.transform.localScale = s, Vector3.one * SPLASH_SIZE, 0.5f));
            seq.Append(DOTween.To(() => splashgo.transform.localScale, (s) => splashgo.transform.localScale = s, Vector3.zero, SPLASH_FADE_TIME));
            seq.OnComplete(() => GameObject.Destroy(splashgo));
        }

        public void Update()
        {
            Shield += def.shieldRegen * Time.deltaTime;
        }
        public bool Overlaps(Vector3 point)
        {
            return GetComponent<CircleCollider2D>().OverlapPoint(point);
        }
    }
}
