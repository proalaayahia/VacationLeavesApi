using System;
using System.Collections.Generic;

namespace VacationLeavesApi.Data;

public partial class Vacation
{
    public short Vaccode { get; set; }

    public string VacationName { get; set; } = null!;

    public string VacationAname { get; set; } = null!;

    public string VacationFname { get; set; } = null!;

    public short Vactype { get; set; }

    public string Legend { get; set; } = null!;

    public int ColorNo { get; set; }

    public bool Postpaycd { get; set; }

    public short Vacbalcat { get; set; }

    public short Vacbalcat1 { get; set; }

    public short Vacbalcat2 { get; set; }

    public bool UseDayFactor { get; set; }

    public decimal DayFactor { get; set; }

    public bool DoNotCountDayoff { get; set; }

    public bool DoNotCountHoliday { get; set; }

    public bool DoNotCountNoShift { get; set; }

    public short YearResetBase { get; set; }

    public short YearlyReset { get; set; }

    public short CalendarBased { get; set; }

    public bool AcceptPartDayFrom { get; set; }

    public bool AcceptPartDayTo { get; set; }

    public decimal PartDayFactorFrom { get; set; }

    public decimal PartDayFactorTo { get; set; }

    public int VacCodeCons { get; set; }

    public string VacationNameCons { get; set; } = null!;

    public string VacationAnameCons { get; set; } = null!;

    public string VacationFnameCons { get; set; } = null!;

    public DateTime? VacationFromDate { get; set; }

    public DateTime? VacationToDate { get; set; }

    public string VacationRoles { get; set; } = null!;

    public short VacationRyear { get; set; }

    public string VacationRno { get; set; } = null!;

    public DateTime? VacationRissueDate { get; set; }

    public DateTime? VacationRexecDate { get; set; }

    public short? VacationRstatus { get; set; }

    public short? VacationRsource { get; set; }

    public string VacationRdesc { get; set; } = null!;

    public Guid VacationGuid { get; set; }
}
