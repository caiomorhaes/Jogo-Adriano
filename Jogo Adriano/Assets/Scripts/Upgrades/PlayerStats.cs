using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Guarda os atributos principais do player e centraliza vida, upgrades, morte e granada.
/// </summary>
public class PlayerStats : MonoBehaviour
{
    [Header("Vida")]
    public float baseHealth = 100f;
    public float currentHealth;

    [Header("Ataque")]
    public int damage = 1;

    private Gun gun;

    [Header("Granada")]
    public float grenadeDamage = 10f;
    public float grenadeRadius = 10f;
    public float grenadeCooldown = 5f;
    public float grenadeSpeed = 16f;
    public GameObject grenadePrefab;

    private PlayerMovement3D movement;
    private List<UpgradeData> activeUpgrades = new List<UpgradeData>();

    private bool morreu;
    private GameObject telaMorte;
    private float proximaGranadaPermitida;

    /// <summary>
    /// Tempo restante para a próxima granada. A HUD usa isso para mostrar o cooldown.
    /// </summary>
    public float GrenadeCooldownRemaining
    {
        get { return Mathf.Max(0f, proximaGranadaPermitida - Time.time); }
    }

    void Start()
    {
        currentHealth = baseHealth;
        movement = GetComponent<PlayerMovement3D>();
        gun = FindAnyObjectByType<Gun>();
    }

    void Update()
    {
        if (morreu || Time.timeScale == 0f)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            TentarArremessarGranada();
        }

        if (currentHealth <= 0f)
        {
            Morrer();
        }
    }

    /// <summary>
    /// Reduz a vida do player e chama a tela de morte quando chega a zero.
    /// </summary>
    public void ReceberDano(float dano)
    {
        if (morreu || currentHealth <= 0f)
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - dano, 0f);
        Debug.Log("Player tomou dano: " + dano + ". Vida atual: " + currentHealth);

    }

    void Morrer()
    {
        morreu = true;
        currentHealth = 0f;
        Time.timeScale = 0f;
        MostrarTelaMorte();

    }

    /// <summary>
    /// Cria a tela de morte em runtime para garantir que ela exista mesmo se não estiver na cena.
    /// </summary>
    void MostrarTelaMorte()
    {
        if (telaMorte != null)
        {
            telaMorte.SetActive(true);
            return;
        }

        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            canvas = CriarCanvas();
        }
        else
        {
            ConfigurarCanvas(canvas);
        }

        CriarEventSystemSeNecessario();

        telaMorte = new GameObject("Tela de Morte", typeof(RectTransform), typeof(Image));
        telaMorte.transform.SetParent(canvas.transform, false);

        RectTransform telaRect = telaMorte.GetComponent<RectTransform>();
        telaRect.anchorMin = Vector2.zero;
        telaRect.anchorMax = Vector2.one;
        telaRect.offsetMin = Vector2.zero;
        telaRect.offsetMax = Vector2.zero;

        Image fundo = telaMorte.GetComponent<Image>();
        fundo.color = new Color(0f, 0f, 0f, 0.8f);

        TextMeshProUGUI titulo = CriarTexto(telaMorte.transform, "VoceMorreu", "Você morreu", 64, FontStyles.Bold);
        RectTransform tituloRect = titulo.GetComponent<RectTransform>();
        tituloRect.anchoredPosition = new Vector2(0f, 70f);
        tituloRect.sizeDelta = new Vector2(700f, 120f);

        Button botaoResetar = CriarBotao(telaMorte.transform);
        botaoResetar.onClick.AddListener(ResetarJogo);
    }

    Canvas CriarCanvas()
    {
        GameObject canvasObject = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        ConfigurarCanvas(canvas);

        return canvas;
    }

    /// <summary>
    /// Mantém qualquer Canvas em Scale With Screen Size para a UI adaptar à resolução.
    /// </summary>
    void ConfigurarCanvas(Canvas canvas)
    {
        CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();

        if (scaler == null)
        {
            scaler = canvas.gameObject.AddComponent<CanvasScaler>();
        }

        if (canvas.GetComponent<GraphicRaycaster>() == null)
        {
            canvas.gameObject.AddComponent<GraphicRaycaster>();
        }

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;
    }

    void CriarEventSystemSeNecessario()
    {
        if (FindFirstObjectByType<EventSystem>() != null)
        {
            return;
        }

        GameObject eventSystemObject = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        eventSystemObject.transform.SetParent(null);
    }

    TextMeshProUGUI CriarTexto(Transform parent, string nome, string texto, float tamanho, FontStyles estilo)
    {
        GameObject textoObject = new GameObject(nome, typeof(RectTransform), typeof(TextMeshProUGUI));
        textoObject.transform.SetParent(parent, false);

        TextMeshProUGUI textoUI = textoObject.GetComponent<TextMeshProUGUI>();
        textoUI.text = texto;
        textoUI.fontSize = tamanho;
        textoUI.fontStyle = estilo;
        textoUI.alignment = TextAlignmentOptions.Center;
        textoUI.color = Color.white;

        RectTransform rect = textoObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        return textoUI;
    }

    Button CriarBotao(Transform parent)
    {
        GameObject botaoObject = new GameObject("BotaoResetar", typeof(RectTransform), typeof(Image), typeof(Button));
        botaoObject.transform.SetParent(parent, false);

        RectTransform botaoRect = botaoObject.GetComponent<RectTransform>();
        botaoRect.anchorMin = new Vector2(0.5f, 0.5f);
        botaoRect.anchorMax = new Vector2(0.5f, 0.5f);
        botaoRect.pivot = new Vector2(0.5f, 0.5f);
        botaoRect.anchoredPosition = new Vector2(0f, -65f);
        botaoRect.sizeDelta = new Vector2(260f, 70f);

        Image botaoImagem = botaoObject.GetComponent<Image>();
        botaoImagem.color = new Color(0.9f, 0.15f, 0.12f, 1f);

        TextMeshProUGUI textoBotao = CriarTexto(botaoObject.transform, "TextoBotao", "Resetar", 32, FontStyles.Bold);
        RectTransform textoRect = textoBotao.GetComponent<RectTransform>();
        textoRect.anchorMin = Vector2.zero;
        textoRect.anchorMax = Vector2.one;
        textoRect.offsetMin = Vector2.zero;
        textoRect.offsetMax = Vector2.zero;

        return botaoObject.GetComponent<Button>();
    }

    void ResetarJogo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Tenta lançar uma granada no inimigo mais próximo, respeitando o cooldown.
    /// </summary>
    void TentarArremessarGranada()
    {
        if (Time.time < proximaGranadaPermitida)
        {
            return;
        }

        Transform alvo = BuscarInimigoMaisProximo();

        if (alvo == null)
        {
            return;
        }

        CriarGranada(alvo);
        proximaGranadaPermitida = Time.time + grenadeCooldown;
    }

    Transform BuscarInimigoMaisProximo()
    {
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Inimigo");
        Transform maisProximo = null;
        float menorDistancia = Mathf.Infinity;

        foreach (GameObject inimigo in inimigos)
        {
            float distancia = Vector3.Distance(transform.position, inimigo.transform.position);

            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                maisProximo = inimigo.transform;
            }
        }

        return maisProximo;
    }

    void CriarGranada(Transform alvo)
    {
        if (grenadePrefab == null)
        {
            Debug.LogWarning("Prefab da granada nao esta configurado no PlayerStats.");
            return;
        }

        Vector3 posicaoInicial = transform.position + Vector3.up * 1.2f;
        GameObject granada = Instantiate(grenadePrefab, posicaoInicial, Quaternion.identity);

        GrenadeProjectile projetil = granada.GetComponent<GrenadeProjectile>();

        if (projetil == null)
        {
            projetil = granada.AddComponent<GrenadeProjectile>();
        }

        projetil.Iniciar(alvo, grenadeDamage, grenadeRadius, grenadeSpeed);
    }

    /// <summary>
    /// Aplica o upgrade escolhido pelo jogador na tela de level up.
    /// </summary>
    public void ApplyUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null)
        {
            Debug.LogError("Upgrade e NULL!");
            return;
        }

        activeUpgrades.Add(upgrade);

        switch (upgrade.type)
        {
            case UpgradeType.Damage:
                damage += (int)upgrade.value;
                Debug.Log("Dano: " + damage);
                break;

            case UpgradeType.MoveSpeed:
                if (movement != null)
                {
                    movement.speed += upgrade.value;
                    movement.speed = Mathf.Clamp(movement.speed, 0f, 20f);
                    Debug.Log("Velocidade: " + movement.speed);
                }
                break;

            case UpgradeType.AttackSpeed:
                Debug.Log("⚡ Aplicando upgrade de velocidade de ataque...");
                if (gun != null)
                {
                    Debug.Log("achei a arma");
                    gun.intervaloTiro -= 0.1f;

                    // 🔥 evita ficar absurdo (importante)
                    gun.intervaloTiro = Mathf.Clamp(gun.intervaloTiro, 0.1f, 10f);

                    Debug.Log("⚡ Intervalo de tiro: " + gun.intervaloTiro);
                }
                break;

            case UpgradeType.MaxHealth:
                currentHealth += upgrade.value;
                Debug.Log("Vida: " + currentHealth);
                break;

            case UpgradeType.GrenadeRadius:
                grenadeRadius += upgrade.value;
                Debug.Log("Raio da granada: " + grenadeRadius);
                break;

            case UpgradeType.GrenadeDamage:
                grenadeDamage += upgrade.value;
                Debug.Log("Dano da granada: " + grenadeDamage);
                break;
        }

        Debug.Log("Upgrade aplicado: " + upgrade.upgradeName);
    }
}
