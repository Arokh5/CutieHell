using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageExchanger : MonoBehaviour {
   
    #region Fields
    [SerializeField]
    private Image mainImage;
    [SerializeField]
    private Image secondaryImage;
    #endregion

    #region PublicMethods

    public void ActivateMainImage()
    {
        mainImage.gameObject.SetActive(true);
        secondaryImage.gameObject.SetActive(false);       
    }
    
    public void ActivateSecondaryImage()
    {
        secondaryImage.gameObject.SetActive(true);
        mainImage.gameObject.SetActive(false);
    }
    #endregion


}
