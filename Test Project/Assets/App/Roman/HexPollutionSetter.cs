using UnityEngine;
using System.Reflection;

public class HexPollutionSetter : MonoBehaviour
{
    [Header("Посилання")]
    [Tooltip("Об'єкт, який містить всі гекси (Hex_0_0, Hex_0_1, і т.д.)")]
    public GameObject hexGridObject;

    [Header("Налаштування")]
    [Tooltip("Встановити значення при старті сцени")]
    public bool setOnStart = true;

    [Header("Базові значення забруднення повітря")]
    public float[] airPollutionBase = { 2f, 3.3f, 2f, 1f, 1.1f, 3f, 2f, 1.4f, 0.45f, 1f, 1.3f, 0.4f };

    [Header("Базові значення забруднення ґрунту")]
    public float[] soilPollutionBase = { 4.2f, 5.6f, 3f, 3.8f, 2.6f, 5.6f, 5.1f, 4.8f, 3f, 3.8f, 3.8f, 3f };

    [Header("Налаштування варіації")]
    [Range(0f, 0.5f)]
    [Tooltip("Процент варіації (0.2 = ±20%)")]
    public float variationPercent = 0.2f;

    void Start()
    {
        if (setOnStart)
        {
            SetPollutionValues();
        }
    }

    [ContextMenu("Встановити значення забруднення")]
    public void SetPollutionValues()
    {
        // Спробуємо знайти hexGridObject якщо він не встановлений
        if (hexGridObject == null)
        {
            hexGridObject = GameObject.Find("HexGridFreeValue");
            if (hexGridObject == null)
            {
                hexGridObject = GameObject.Find("HexGrid");
            }
            
            if (hexGridObject == null)
            {
                Debug.LogError("❌ HexGrid об'єкт не знайдено! Перетягни об'єкт з гексами в поле 'Hex Grid Object' в інспекторі.");
                return;
            }
        }

        int successCount = 0;
        int errorCount = 0;

        Debug.Log("🚀 Починаємо встановлення значень забруднення...");

        // Проходимо по всіх гексах (6 рядків × 8 колонок)
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                string hexName = $"Hex_{row}_{col}";
                Transform hexTransform = hexGridObject.transform.Find(hexName);

                if (hexTransform == null)
                {
                    Debug.LogWarning($"⚠️ Гекс {hexName} не знайдено!");
                    errorCount++;
                    continue;
                }

                // Визначаємо групу (3×4 сітка значень)
                int groupRow = row / 2; // 0, 1, або 2
                int groupCol = col / 2; // 0, 1, 2, або 3
                int groupIndex = groupRow * 4 + groupCol; // 0-11

                // Отримуємо базові значення для цієї групи
                float baseAir = airPollutionBase[groupIndex];
                float baseSoil = soilPollutionBase[groupIndex];

                // Генеруємо унікальні варіації
                float airVariation = Random.Range(-variationPercent, variationPercent);
                float soilVariation = Random.Range(-variationPercent, variationPercent);

                float finalAir = Mathf.Clamp(baseAir * (1f + airVariation), 0f, 10f);
                float finalSoil = Mathf.Clamp(baseSoil * (1f + soilVariation), 0f, 10f);

                // Встановлюємо значення через Reflection
                GameObject hexObject = hexTransform.gameObject;
                Component[] components = hexObject.GetComponents<Component>();

                bool airSet = false;
                bool soilSet = false;

                // Шукаємо компонент з потрібними властивостями
                foreach (Component comp in components)
                {
                    if (comp is MonoBehaviour)
                    {
                        bool tempAir = SetPropertyValue(comp, "AirPollution", finalAir);
                        bool tempSoil = SetPropertyValue(comp, "SoilPollution", finalSoil);

                        if (tempAir) airSet = true;
                        if (tempSoil) soilSet = true;
                    }
                }

                if (airSet && soilSet)
                {
                    Debug.Log($"✅ {hexName} (Група {groupIndex}): Air={finalAir:F2} (база {baseAir:F1}), Soil={finalSoil:F2} (база {baseSoil:F1})");
                    successCount++;
                }
                else
                {
                    Debug.LogWarning($"⚠️ {hexName}: Не вдалося встановити властивості (AirSet:{airSet}, SoilSet:{soilSet})");
                    errorCount++;
                }
            }
        }

        Debug.Log($"🎉 <color=green>Завершено!</color> Оновлено: {successCount} гексів | Помилки: {errorCount}");
    }

    [ContextMenu("Скинути всі значення на 0")]
    public void ResetPollutionValues()
    {
        if (hexGridObject == null)
        {
            Debug.LogError("❌ HexGrid об'єкт не встановлений!");
            return;
        }

        int count = 0;
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                string hexName = $"Hex_{row}_{col}";
                Transform hexTransform = hexGridObject.transform.Find(hexName);

                if (hexTransform != null)
                {
                    GameObject hexObject = hexTransform.gameObject;
                    Component[] components = hexObject.GetComponents<Component>();

                    foreach (Component comp in components)
                    {
                        if (comp is MonoBehaviour)
                        {
                            SetPropertyValue(comp, "AirPollution", 0f);
                            SetPropertyValue(comp, "SoilPollution", 0f);
                        }
                    }
                    count++;
                }
            }
        }

        Debug.Log($"🔄 Скинуто значення для {count} гексів");
    }

    // Допоміжний метод для встановлення значення властивості через Reflection
    private bool SetPropertyValue(object obj, string propertyName, float value)
    {
        System.Type type = obj.GetType();
        
        // Спробуємо знайти публічну властивість
        PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (property != null && property.CanWrite)
        {
            property.SetValue(obj, value);
            return true;
        }

        // Якщо це auto-property з backing field
        FieldInfo backingField = type.GetField($"<{propertyName}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
        if (backingField != null)
        {
            backingField.SetValue(obj, value);
            return true;
        }

        return false;
    }
}