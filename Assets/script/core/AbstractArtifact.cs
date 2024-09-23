public abstract class AbstractArtifact : MASAbstract
{

    protected string artifactProperties;
    protected AgentArtifactTypeEnum type;

    public string ArtifactProperties
    {
        get { return artifactProperties; }
    }

    public AgentArtifactTypeEnum Type
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