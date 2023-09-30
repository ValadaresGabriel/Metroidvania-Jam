using UnityEngine;
using UnityEngine.UI;

namespace TS
{
    public class MeshMaskUI : MaskableGraphic
    {

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            Vector3 vec00 = new(0, 0);
            Vector3 vec01 = new(0, 50);

            Vector3 vec10 = new(50 + Time.realtimeSinceStartup * 10f, 0);
            Vector3 vec11 = new(50 + Time.realtimeSinceStartup * 10f, 50);

            vh.AddUIVertexQuad(new UIVertex[]
            {
                new UIVertex { position = vec00, color = Color.green },
                new UIVertex { position = vec01, color = Color.green },
                new UIVertex { position = vec10, color = Color.green },
                new UIVertex { position = vec11, color = Color.green },
            });
        }

        private void Update()
        {
            SetVerticesDirty();
        }
    }
}
