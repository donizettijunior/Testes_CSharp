namespace Utils
{
    public class LibUtils
    {
        public bool ValidaOpcao(string opc)
        {
            if ((opc == null) || (opc.Length == 0) || (opc.Length > 1))
                return false;
            try
            {
                int opcao = int.Parse(opc);
                if (opcao <= 6)
                    return true;
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
