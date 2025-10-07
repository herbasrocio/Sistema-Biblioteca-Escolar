public class Familia : Component
{
    private List<Component> childrens;
    public string Nombre { get; set; }
    // Agregar esta propiedad para exponer el IdComponent como IdFamilia
    public Guid IdFamilia
    {
        get { return this.IdComponent; }
    }
    public List<Component> GetChildrens();
    public override void Add(Component component);
    public override int ChildrenCount();
    public override void Remove(Component component);
}