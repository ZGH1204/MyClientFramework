using UnityEngine;

public class FPSFireManager : MonoBehaviour
{
    //子弹
    public ImpactInfo[] ImpactElemets = new ImpactInfo[0];

    // FPS相机
    public Camera fpsCam;

    //子弹距离
    public float BulletDistance = 100;

    // 枪击音效
    public AudioSource gunAudio;

    //
    //public GameObject ImpactEffect;

    // 设置两次枪击的间隔时间
    public float fireRate = 0.25f;

    private float shootTime = -1f;

    private void LateUpdate()
    {
        shootTime -= Time.deltaTime;
    }

    public void Shoot()
    {
        if (shootTime > 0)
        {
            return;
        }

        // 在相机视口中心创建向量
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        RaycastHit hit;
        var ray = new Ray(rayOrigin, fpsCam.transform.forward);
        if (Physics.Raycast(ray, out hit, BulletDistance))
        {
            var effect = GetImpactEffect(hit.transform.gameObject);
            if (effect == null)
                return;
            var effectIstance = Instantiate(effect, hit.point, new Quaternion()) as GameObject;
            //ImpactEffect.SetActive(false);
            //ImpactEffect.SetActive(true);
            effectIstance.transform.LookAt(hit.point + hit.normal);
            Destroy(effectIstance, 4);
            gunAudio.Play();
        }

        shootTime = fireRate;
    }

    [System.Serializable]
    public class ImpactInfo
    {
        public MaterialType.MaterialTypeEnum MaterialType;
        public GameObject ImpactEffect;
    }

    private GameObject GetImpactEffect(GameObject impactedGameObject)
    {
        var materialType = impactedGameObject.GetComponent<MaterialType>();
        if (materialType == null)
            return null;
        foreach (var impactInfo in ImpactElemets)
        {
            if (impactInfo.MaterialType == materialType.TypeOfMaterial)
                return impactInfo.ImpactEffect;
        }
        return null;
    }
}