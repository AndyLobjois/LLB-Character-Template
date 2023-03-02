using System;

namespace Cactuar
{
    public class Floatf
    {
        public decimal ToDecimal(HHBCPNCDNDH val1)
        {
            return val1.OHJPAPMBBKM / 4294967296m;
        }

        public HHBCPNCDNDH DecimalToFloatf(Decimal n)
        {
            return HHBCPNCDNDH.NKKIFJJEPOL(n);
        }

        public HHBCPNCDNDH One()
        {
            return HHBCPNCDNDH.GNIKMEGGCEP;
        }

        public HHBCPNCDNDH Divide(HHBCPNCDNDH val1, HHBCPNCDNDH val2)
        {
            return HHBCPNCDNDH.FCGOICMIBEA(val1, val2);
        }

        public HHBCPNCDNDH Multiply(HHBCPNCDNDH val1, HHBCPNCDNDH val2)
        {
            return HHBCPNCDNDH.AJOCFFLIIIH(val1, val2);
        }

        public HHBCPNCDNDH Negative(HHBCPNCDNDH n)
        {
            return HHBCPNCDNDH.GANELPBAOPN(n);
        }

        public bool GreaterThan(HHBCPNCDNDH val1, HHBCPNCDNDH val2)
        {
            return HHBCPNCDNDH.OAHDEOGKOIM(val1, val2);
        }

        public HHBCPNCDNDH Subtract(HHBCPNCDNDH val1, HHBCPNCDNDH val2)
        {
            return HHBCPNCDNDH.FCKBPDNEAOG(val1, val2);
        }

        public HHBCPNCDNDH Modulus(HHBCPNCDNDH val1, HHBCPNCDNDH val2)
        {
            return HHBCPNCDNDH.CFIEILGNOBP(val1, val2);
        }
    }
}
