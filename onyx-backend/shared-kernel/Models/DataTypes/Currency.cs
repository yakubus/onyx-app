using Models.Responses;

namespace Models.DataTypes;

public sealed record Currency
{
    public static readonly Currency Afn = new("AFN");
    public static readonly Currency Amd = new("AMD");
    public static readonly Currency Aoa = new("AOA");
    public static readonly Currency Ars = new("ARS");
    public static readonly Currency Aud = new("AUD");
    public static readonly Currency Awg = new("AWG");
    public static readonly Currency Azn = new("AZN");
    public static readonly Currency Bam = new("BAM");
    public static readonly Currency Bbd = new("BBD");
    public static readonly Currency Bdt = new("BDT");
    public static readonly Currency Bgn = new("BGN");
    public static readonly Currency Bhd = new("BHD");
    public static readonly Currency Bif = new("BIF");
    public static readonly Currency Bmd = new("BMD");
    public static readonly Currency Bnd = new("BND");
    public static readonly Currency Bob = new("BOB");
    public static readonly Currency Brl = new("BRL");
    public static readonly Currency Bsd = new("BSD");
    public static readonly Currency Btc = new("BTC");
    public static readonly Currency Btn = new("BTN");
    public static readonly Currency Bwp = new("BWP");
    public static readonly Currency Byr = new("BYR");
    public static readonly Currency Bzd = new("BZD");
    public static readonly Currency Cad = new("CAD");
    public static readonly Currency Cdf = new("CDF");
    public static readonly Currency Clf = new("CLF");
    public static readonly Currency Clp = new("CLP");
    public static readonly Currency Cny = new("CNY");
    public static readonly Currency Cop = new("COP");
    public static readonly Currency Crc = new("CRC");
    public static readonly Currency Cuc = new("CUC");
    public static readonly Currency Cup = new("CUP");
    public static readonly Currency Cve = new("CVE");
    public static readonly Currency Czk = new("CZK");
    public static readonly Currency Hrk = new("HRK");
    public static readonly Currency Khr = new("KHR");
    public static readonly Currency Kmf = new("KMF");
    public static readonly Currency Kyd = new("KYD");
    public static readonly Currency Xaf = new("XAF");
    public static readonly Currency Xof = new("XOF");
    public static readonly Currency Xpf = new("XPF");
    public static readonly Currency Djf = new("DJF");
    public static readonly Currency Dkk = new("DKK");
    public static readonly Currency Dop = new("DOP");
    public static readonly Currency Egp = new("EGP");
    public static readonly Currency Ern = new("ERN");
    public static readonly Currency Etb = new("ETB");
    public static readonly Currency Eur = new("EUR");
    public static readonly Currency Xcd = new("XCD");
    public static readonly Currency Fjd = new("FJD");
    public static readonly Currency Fkp = new("FKP");
    public static readonly Currency Gel = new("GEL");
    public static readonly Currency Ggp = new("GGP");
    public static readonly Currency Ghs = new("GHS");
    public static readonly Currency Gip = new("GIP");
    public static readonly Currency Gmd = new("GMD");
    public static readonly Currency Gnf = new("GNF");
    public static readonly Currency Gtq = new("GTQ");
    public static readonly Currency Gyd = new("GYD");
    public static readonly Currency Xau = new("XAU");
    public static readonly Currency Hkd = new("HKD");
    public static readonly Currency Hnl = new("HNL");
    public static readonly Currency Htg = new("HTG");
    public static readonly Currency Huf = new("HUF");
    public static readonly Currency Idr = new("IDR");
    public static readonly Currency Ils = new("ILS");
    public static readonly Currency Inr = new("INR");
    public static readonly Currency Iqd = new("IQD");
    public static readonly Currency Irr = new("IRR");
    public static readonly Currency Isk = new("ISK");
    public static readonly Currency Jep = new("JEP");
    public static readonly Currency Jmd = new("JMD");
    public static readonly Currency Jod = new("JOD");
    public static readonly Currency Jpy = new("JPY");
    public static readonly Currency Kes = new("KES");
    public static readonly Currency Kgs = new("KGS");
    public static readonly Currency Kwd = new("KWD");
    public static readonly Currency Kzt = new("KZT");
    public static readonly Currency Lak = new("LAK");
    public static readonly Currency Lbp = new("LBP");
    public static readonly Currency Lrd = new("LRD");
    public static readonly Currency Lsl = new("LSL");
    public static readonly Currency Ltl = new("LTL");
    public static readonly Currency Lvl = new("LVL");
    public static readonly Currency Lyd = new("LYD");
    public static readonly Currency Imp = new("IMP");
    public static readonly Currency Mad = new("MAD");
    public static readonly Currency Mdl = new("MDL");
    public static readonly Currency Mga = new("MGA");
    public static readonly Currency Mkd = new("MKD");
    public static readonly Currency Mmk = new("MMK");
    public static readonly Currency Mnt = new("MNT");
    public static readonly Currency Mop = new("MOP");
    public static readonly Currency Mro = new("MRO");
    public static readonly Currency Mur = new("MUR");
    public static readonly Currency Mvr = new("MVR");
    public static readonly Currency Mwk = new("MWK");
    public static readonly Currency Mxn = new("MXN");
    public static readonly Currency Myr = new("MYR");
    public static readonly Currency Mzn = new("MZN");
    public static readonly Currency Ang = new("ANG");
    public static readonly Currency Byn = new("BYN");
    public static readonly Currency Kpw = new("KPW");
    public static readonly Currency Nad = new("NAD");
    public static readonly Currency Ngn = new("NGN");
    public static readonly Currency Nio = new("NIO");
    public static readonly Currency Nok = new("NOK");
    public static readonly Currency Npr = new("NPR");
    public static readonly Currency Nzd = new("NZD");
    public static readonly Currency Twd = new("TWD");
    public static readonly Currency Omr = new("OMR");
    public static readonly Currency Pab = new("PAB");
    public static readonly Currency Pen = new("PEN");
    public static readonly Currency Pkg = new("PGK");
    public static readonly Currency Php = new("PHP");
    public static readonly Currency Pkr = new("PKR");
    public static readonly Currency Pln = new("PLN");
    public static readonly Currency Pyg = new("PYG");
    public static readonly Currency Qar = new("QAR");
    public static readonly Currency Ron = new("RON");
    public static readonly Currency Rub = new("RUB");
    public static readonly Currency Rwf = new("RWF");
    public static readonly Currency Chf = new("CHF");
    public static readonly Currency Krw = new("KRW");
    public static readonly Currency Lkr = new("LKR");
    public static readonly Currency Rsd = new("RSD");
    public static readonly Currency Sar = new("SAR");
    public static readonly Currency Sbd = new("SBD");
    public static readonly Currency Scr = new("SCR");
    public static readonly Currency Sdg = new("SDG");
    public static readonly Currency Sek = new("SEK");
    public static readonly Currency Sgd = new("SGD");
    public static readonly Currency Shp = new("SHP");
    public static readonly Currency Sll = new("SLL");
    public static readonly Currency Sos = new("SOS");
    public static readonly Currency Srd = new("SRD");
    public static readonly Currency Std = new("STD");
    public static readonly Currency Svc = new("SVC");
    public static readonly Currency Syp = new("SYP");
    public static readonly Currency Szl = new("SZL");
    public static readonly Currency Wst = new("WST");
    public static readonly Currency Xag = new("XAG");
    public static readonly Currency Xdr = new("XDR");
    public static readonly Currency Zar = new("ZAR");
    public static readonly Currency Thb = new("THB");
    public static readonly Currency Tjs = new("TJS");
    public static readonly Currency Tmt = new("TMT");
    public static readonly Currency Tnd = new("TND");
    public static readonly Currency Top = new("TOP");
    public static readonly Currency Try = new("TRY");
    public static readonly Currency Ttd = new("TTD");
    public static readonly Currency Tzs = new("TZS");
    public static readonly Currency Aed = new("AED");
    public static readonly Currency Uah = new("UAH");
    public static readonly Currency Ugx = new("UGX");
    public static readonly Currency Usd = new("USD");
    public static readonly Currency Uyu = new("UYU");
    public static readonly Currency Uzs = new("UZS");
    public static readonly Currency Vef = new("VEF");
    public static readonly Currency Vnd = new("VND");
    public static readonly Currency Vuv = new("VUV");
    public static readonly Currency Yer = new("YER");
    public static readonly Currency Zmk = new("ZMK");
    public static readonly Currency Zmw = new("ZMW");
    public static readonly Currency Zwl = new("ZWL");

    public Currency()
    {
    }

    private Currency(string code) => Code = code;

    public string Code { get; init; }

    public static Result<Currency> FromCode(string code)
    {
        return All.FirstOrDefault(c => c.Code.ToLower() == code.ToLower()) ??
               Result.Failure<Currency>(new ("Currency.NotFoune", "Currency not found"));
    }

    public static readonly IReadOnlyCollection<Currency> All = new[]
    {
        Pln,
        Usd,
        Afn,
        Amd,
        Aoa,
        Ars,
        Aud,
        Awg,
        Azn,
        Bam,
        Bbd,
        Bdt,
        Bgn,
        Bhd,
        Bif,
        Bmd,
        Bnd,
        Bob,
        Brl,
        Bsd,
        Btc,
        Btn,
        Bwp,
        Byr,
        Bzd,
        Cad,
        Cdf,
        Clf,
        Clp,
        Cny,
        Cop,
        Crc,
        Cuc,
        Cup,
        Cve,
        Czk,
        Hrk,
        Khr,
        Kmf,
        Kyd,
        Xaf,
        Xof,
        Xpf,
        Djf,
        Dkk,
        Dop,
        Egp,
        Ern,
        Etb,
        Eur,
        Xcd,
        Fjd,
        Fkp,
        Gel,
        Ggp,
        Ghs,
        Gip,
        Gmd,
        Gnf,
        Gtq,
        Gyd,
        Xau,
        Hkd,
        Hnl,
        Htg,
        Huf,
        Idr,
        Ils,
        Inr,
        Iqd,
        Irr,
        Isk,
        Jep,
        Jmd,
        Jod,
        Jpy,
        Kes,
        Kgs,
        Kwd,
        Kzt,
        Lak,
        Lbp,
        Lrd,
        Lsl,
        Ltl,
        Lvl,
        Lyd,
        Imp,
        Mad,
        Mdl,
        Mga,
        Mkd,
        Mmk,
        Mnt,
        Mop,
        Mro,
        Mur,
        Mvr,
        Mwk,
        Mxn,
        Myr,
        Mzn,
        Ang,
        Byn,
        Kpw,
        Nad,
        Ngn,
        Nio,
        Nok,
        Npr,
        Nzd,
        Twd,
        Omr,
        Pab,
        Pen,
        Pkg,
        Php,
        Pkr,
        Pyg,
        Qar,
        Ron,
        Rub,
        Rwf,
        Chf,
        Krw,
        Lkr,
        Rsd,
        Sar,
        Sbd,
        Scr,
        Sdg,
        Sek,
        Sgd,
        Shp,
        Sll,
        Sos,
        Srd,
        Std,
        Svc,
        Syp,
        Szl,
        Wst,
        Xag,
        Xdr,
        Zar,
        Thb,
        Tjs,
        Tmt,
        Tnd,
        Top,
        Try,
        Ttd,
        Tzs,
        Aed,
        Uah,
        Ugx,
        Uyu,
        Uzs,
        Vef,
        Vnd,
        Vuv,
        Yer,
        Zmk,
        Zmw,
        Zwl
    };
}