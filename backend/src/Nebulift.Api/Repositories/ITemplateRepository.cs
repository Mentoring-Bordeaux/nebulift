namespace Nebulift.Api.Templates;

using Types;

public interface ITemplateRepository {

    public TIdentity GetTemplateIdentity(string id);

    public TInputs GetTemplateInputs(string id);

    public TOutputs GetTemplateOutputs(string id);
}