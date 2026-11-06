using UnityEditor.Rendering.LookDev;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AdvancedCharacterCore : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public Transform playerVisuals; // Küpün içindeki görsel obje (Örn: Model)

    [Header("Settings")]
    public float movementSpeed = 7f;
    public float rotationSpeed = 10f;
    public bool useSlerp = true; // Inspector'dan açıp kapatarak farkı görün

    private Rigidbody _rb;
    private Vector3 _inputDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        // Karakterin yuvarlanmaması için fiziksel rotasyonu kilitliyoruz (Sadece pozisyon fizikle değişecek)
        _rb.constraints = RigidbodyConstraints.FreezeRotation; 
    }

    private void Update()
    {
        // 1. INPUT OKUMA (Update içinde yapılmalı)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 2. KAMERA YÖNÜNÜ YERE İZDÜŞÜRME (Ekstra Trick: ProjectOnPlane)
        // Kameranın ileri yönünü (Z), Y düzlemine (Vector3.up) göre düzleştiriyoruz.
        Vector3 camForwardOnPlane = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 camRightOnPlane = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

        // 3. GİDİLECEK YÖNÜ HESAPLAMA
        _inputDirection = (camForwardOnPlane * v) + (camRightOnPlane * h);
        _inputDirection.Normalize();

        // 4. GÖRSEL ROTASYON (Fizik değil, sadece modelin kendi etrafında dönmesi)
        if (_inputDirection.sqrMagnitude > 0.01f) // Vektör sıfır değilse (Sıfırsa hata verir)
        {
            // Vektörü bir rotasyona (Quaternion) çeviriyoruz
            Quaternion targetRotation = Quaternion.LookRotation(_inputDirection);

            // Lerp ve Slerp farkını test edebilmeniz için
            if (useSlerp)
                playerVisuals.rotation = Quaternion.Slerp(playerVisuals.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            else
                playerVisuals.rotation = Quaternion.Lerp(playerVisuals.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void FixedUpdate()
    {
        // 5. FİZİKSEL HAREKET (FixedUpdate içinde yapılmalı)
        // Karakterin mevcut Y hızını koruyoruz (Yerçekimi bozulmasın diye)
        Vector3 targetVelocity = _inputDirection * movementSpeed;
        targetVelocity.y = _rb.linearVelocity.y; 

        _rb.linearVelocity = targetVelocity;
    }

    // 6. GÖRSELLEŞTİRME (GİZMOS) - Sahnede arka plan matematiğini çizer
    private void OnDrawGizmos()
    {
        if (cameraTransform == null || playerVisuals == null) return;

        // Kameranın İleri Yönü (Mavi) - Yere paralel hale getirilmiş
        Vector3 camForwardOnPlane = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Gizmos.color = Color.cyan;
        DrawArrow(transform.position, camForwardOnPlane * 2f);

        // Oyuncunun Gitmek İstediği Yön (Yeşil)
        Gizmos.color = Color.green;
        DrawArrow(transform.position, _inputDirection * 3f);

        // Oyuncu Modelinin Mevcut Baktığı Yön (Kırmızı)
        Gizmos.color = Color.red;
        DrawArrow(transform.position, playerVisuals.forward * 3f);
    }

    // Gizmoların ucuna ok başı çizen yardımcı metot
    private void DrawArrow(Vector3 start, Vector3 direction)
    {
        Gizmos.DrawRay(start, direction);
        Vector3 end = start + direction;
        Gizmos.DrawSphere(end, 0.1f); // Vektörün ucunu belli etmek için
    }
       void PlayerMovement()
    {
        float v=Input.GetAxisRaw("Vertical");
        float h=Input.GetAxisRaw("Horizontal");
       
    }
}