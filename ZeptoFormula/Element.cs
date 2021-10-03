
namespace ZeptoFormula
{
    public struct Element
    {
        public ElementType elementType;
        public int val;
        public Element(ElementType elementType)
        {
            this.elementType = elementType;
            this.val = 0;
        }
        public Element(int val)
        {
            this.elementType = ElementType.CONST;
            this.val = val;
        }
        public static Element BlankElement = new Element(ElementType.NONE);

        public override string ToString()
        {
            if (val != 0 || elementType == ElementType.CONST) 
            {
                return elementType.ToString()+"="+val;
            }
            else 
            {
                return elementType.ToString();
            }
        }
    }
}