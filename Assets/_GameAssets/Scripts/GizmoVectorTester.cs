using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AdvancedTechniquesPlayground : MonoBehaviour
{
    public enum MethodType
    {
        TransformTranslate,     // Lokal İleri Hareket
        VectorMoveTowards,      // Sabit hızlı hedefe gitme
        VectorSmoothDamp,       // Yaylanarak yumuşak takip (Kamera mantığı)
        RigidbodyAddForce,      // Zıplama/İtme gücü
        RigidbodyMovePosition,  // Güvenli Fiziksel Yürüme
        QuaternionRotateTowards // Sabit Hızlı Taret Dönüşü
    }

    [Header("Ayarlar")]
    public MethodType currentMethod;
    public Transform targetObj; // Hedef obje (Küre)
    public float speed = 10f;
    
    [Header("SmoothDamp Ayarları")]
    public float smoothTime = 0.3f; // Hedefe varış süresi
    private Vector3 _currentVelocity; // SmoothDamp'in arka planda kullandığı hafıza değişkeni

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 1. TRANSFORM İŞLEMLERİ (Fizik motorunu es geçer)
        switch (currentMethod)
        {
            case MethodType.TransformTranslate:
                // W tuşuna basınca objenin "kendi ileri yönüne (Lokal Z)" gitmesini sağlar.
                float v = Input.GetAxis("Vertical");
                transform.Translate(Vector3.forward * v * speed * Time.deltaTime, Space.Self);
                break;

            case MethodType.VectorMoveTowards:
                // Hedef objeye (Target) asla yavaşlamadan sabit hızla robotik şekilde gider
                if (targetObj != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetObj.position, speed * Time.deltaTime);
                }
                break;

            case MethodType.VectorSmoothDamp:
                // Hedefe giderken yaylanır. Kamera takiplerinin %90'ı bununla yazılır.
                // ref _currentVelocity: Hız referansını sisteme veriyoruz ki ivmeyi kendisi hesaplasın
                if (targetObj != null)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, targetObj.position, ref _currentVelocity, smoothTime);
                }
                break;

            case MethodType.QuaternionRotateTowards:
                // Hedefe bakacak açıyı (LookRotation) hesapla, ama Slerp gibi yavaşlayarak değil,
                // tank namlusu gibi sabit hızla dön (saniyede 'speed' derece)
                if (targetObj != null)
                {
                    Vector3 directionToTarget = (targetObj.position - transform.position).normalized;
                    Quaternion targetRot = Quaternion.LookRotation(directionToTarget);
                    
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, speed * 10f * Time.deltaTime);
                }
                break;
        }

        // AddForce Zıplama Testi (Sadece Boşluk Tuşuna Basılınca)
        if (currentMethod == MethodType.RigidbodyAddForce && Input.GetKeyDown(KeyCode.Space))
        {
            // Impulse: Bir anlık devasa bir patlama/zıplama gücü verir.
            _rb.AddForce(Vector3.up * speed, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // 2. RIGIDBODY İŞLEMLERİ (Fizik Döngüsü)
        if (currentMethod == MethodType.RigidbodyMovePosition && targetObj != null)
        {
            // Hedefe doğru gitmek için "Arka planda ne kadar Velocity gerekiyor" hesabını kendi yapar.
            // Duvara çarptığında Transform gibi delip geçmez, durur.
            Vector3 newPos = Vector3.MoveTowards(_rb.position, targetObj.position, speed * Time.fixedDeltaTime);
            _rb.MovePosition(newPos);
        }
    }

    private void OnDrawGizmos()
    {
        if (targetObj == null) return;

        // Hedef objeye giden rotayı çizer (Turuncu)
        Gizmos.color = new Color(1f, 0.5f, 0f); // Turuncu
        Gizmos.DrawLine(transform.position, targetObj.position);

        // Eğer SmoothDamp seçiliyse, o anki anlık Hız/İvme vektörünü (Spring enerjisini) Beyaz olarak çizer
        if (currentMethod == MethodType.VectorSmoothDamp)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawRay(transform.position, _currentVelocity);
        }
    }
}