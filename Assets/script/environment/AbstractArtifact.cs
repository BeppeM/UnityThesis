public abstract class AbstractArtifact : MASAbstract
{

    protected string artifactProperties;
    protected ArtifactTypeEnum type;

    public string ArtifactProperties
    {
        get { return artifactProperties; }
    }

    public ArtifactTypeEnum Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
        }
    }

}