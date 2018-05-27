using UnityEngine;
using UnityEngine.UI;

public class EnemyDescriptionController : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text descriptionText;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(nameText, "ERROR: The EnemyDescriptionController in gameObject '" + gameObject.name + "' doesn't have a Text (nameText) assigned!");
        UnityEngine.Assertions.Assert.IsNotNull(descriptionText, "ERROR: The EnemyDescriptionController in gameObject '" + gameObject.name + "' doesn't have a Text (descriptionText) assigned!");
        gameObject.SetActive(false);
    }
    #endregion

    #region Public Methods
    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetDescription(string description)
    {
        descriptionText.text = description;
    }
    #endregion
}
