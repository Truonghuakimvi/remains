using UnityEngine;

public class BuildManager : MonoBehaviour
{
	public static BuildManager instance;
    void Awake()
    {
        instance = this;
    }

    public Shop shop;

    public GameObject buildEffect;

    private TurretBlueprint turretToBuild;
    public GameObject turretToBuildButton;

    private Node selectedNode;
    public NodeUI nodeUI;

    public GameObject preview;
    public AudioSource buildSFX;
    public AudioSource retreatSFX;
    public AudioSource errorSFX;

    public bool isMapFlipped;

    public bool turretIsSelected
    {
        get { return turretToBuild != null; }
    }

    public void SelectTurretToBuild(TurretBlueprint turret, GameObject turretButton)
    {
        turretToBuild = turret;
        turretToBuildButton = turretButton;
        DeselectNode();
        PreviewTurretDestroy();
    }

    private void SetGameLayerRecursive(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
        {
            SetGameLayerRecursive(child.gameObject, layer);
        }
    }

    public void PreviewTurretOn()
    {
        PreviewTurretDestroy();
        GameObject turretPreview = Instantiate(turretToBuild.prefab, preview.transform.position, Quaternion.identity);
        if (isMapFlipped)
        {
            turretPreview.transform.localScale = new Vector3(-turretPreview.transform.localScale.x, turretPreview.transform.localScale.y, turretPreview.transform.localScale.z);
        }
        SetGameLayerRecursive(turretPreview, LayerMask.NameToLayer("Top Layer"));
        if (turretPreview.GetComponent<Turret>() != null)
        {
            turretPreview.GetComponent<Turret>().enabled = false;
        }
        else
        {
            turretPreview.GetComponent<Healer>().enabled = false;
        }     
        turretPreview.GetComponent<Animator>().enabled = false;
        turretPreview.tag = "Untagged";

        turretPreview.transform.parent = preview.transform;
        preview.GetComponent<TurretPreview>().turret = turretPreview;
    }

    public void PreviewTurretDestroy()
    {
        if (preview.GetComponent<TurretPreview>().turret != null)
        {
            Destroy(preview.transform.GetChild(0).gameObject);
        }
    }

    public void PreviewTurretOnNode(Node node, bool enterNode)
    {
        if (enterNode == true && node.turret == null)
        {
            preview.transform.position = node.transform.position;
            preview.GetComponent<TurretPreview>().isOnNode = true;
        }
        else
        {
            preview.GetComponent<TurretPreview>().isOnNode = false;
        }
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void SelectedNode(Node node)
    {
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }

        selectedNode = node;
        turretToBuild = null;
        PreviewTurretDestroy();

        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    public void PlayBuildSFX()
    {
        buildSFX.Play();
    }
    public void PlayRetreatSFX()
    {
        retreatSFX.Play();
    }

    public void PlayErrorSound()
    {
        errorSFX.Play();
    }
}
