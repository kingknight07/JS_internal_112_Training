using System;
using System.Collections.Generic;

// ══════════════════════════════════════════════════════════════════════════════
//                    HOSPITAL PATIENT MANAGEMENT SYSTEM
//                  Demonstrating C# OOP, Delegates & Events
// ══════════════════════════════════════════════════════════════════════════════

namespace HospitalManagement
{
    #region ═══════════════ MODELS (OOP - Abstraction, Inheritance, Polymorphism) ═══════════════

    /// <summary>
    /// Abstract base class - Demonstrates ABSTRACTION & ENCAPSULATION
    /// </summary>
    public abstract class Patient
    {
        // ENCAPSULATION - Properties with controlled access
        public string PatientId { get; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime AdmissionDate { get; }
        public string PatientType => GetType().Name.Replace("Patient", "");

        // Protected constructor - only derived classes can use
        protected Patient(string patientId, string name, int age)
        {
            PatientId = patientId;
            Name = name;
            Age = age;
            AdmissionDate = DateTime.Now;
        }

        // ABSTRACTION - Abstract method must be implemented by derived classes
        public abstract decimal GetBaseTreatmentCost();

        // POLYMORPHISM - Virtual method can be overridden
        public virtual string GetPatientInfo()
        {
            return $"[{PatientType}] {Name} (ID: {PatientId}) | Age: {Age}";
        }
    }

    /// <summary>
    /// INHERITANCE - GeneralPatient inherits from Patient
    /// </summary>
    public class GeneralPatient : Patient
    {
        public string Diagnosis { get; set; }

        public GeneralPatient(string patientId, string name, int age, string diagnosis)
            : base(patientId, name, age)
        {
            Diagnosis = diagnosis;
        }

        // POLYMORPHISM - Override abstract method
        public override decimal GetBaseTreatmentCost() => 5000m;

        public override string GetPatientInfo()
        {
            return $"{base.GetPatientInfo()} | Diagnosis: {Diagnosis}";
        }
    }

    /// <summary>
    /// INHERITANCE - EmergencyPatient inherits from Patient
    /// </summary>
    public class EmergencyPatient : Patient
    {
        public string EmergencyType { get; set; }
        public int SeverityLevel { get; set; }

        public EmergencyPatient(string patientId, string name, int age, string emergencyType, int severity)
            : base(patientId, name, age)
        {
            EmergencyType = emergencyType;
            SeverityLevel = Math.Clamp(severity, 1, 5);
        }

        // POLYMORPHISM - Different cost calculation
        public override decimal GetBaseTreatmentCost() => 15000m + (SeverityLevel * 2000m);

        public override string GetPatientInfo()
        {
            return $"{base.GetPatientInfo()} | Emergency: {EmergencyType} | Severity: {SeverityLevel}/5";
        }
    }

    /// <summary>
    /// INHERITANCE - ICUPatient inherits from Patient
    /// </summary>
    public class ICUPatient : Patient
    {
        public int ICUDays { get; set; }
        public bool RequiresVentilator { get; set; }

        public ICUPatient(string patientId, string name, int age, int icuDays, bool ventilator)
            : base(patientId, name, age)
        {
            ICUDays = icuDays;
            RequiresVentilator = ventilator;
        }

        // POLYMORPHISM - Complex cost calculation
        public override decimal GetBaseTreatmentCost()
        {
            decimal baseCost = 25000m + (ICUDays * 8000m);
            return RequiresVentilator ? baseCost + 15000m : baseCost;
        }

        public override string GetPatientInfo()
        {
            return $"{base.GetPatientInfo()} | ICU Days: {ICUDays} | Ventilator: {(RequiresVentilator ? "Yes" : "No")}";
        }
    }

    #endregion

    #region ═══════════════ DELEGATES (Dynamic Billing Strategy) ═══════════════

    /// <summary>
    /// DELEGATE - Type-safe function pointer for billing strategies
    /// </summary>
    public delegate decimal BillingStrategy(decimal baseCost);

    /// <summary>
    /// Bill record - using struct for simplicity
    /// </summary>
    public class Bill
    {
        public string BillId { get; set; }
        public Patient Patient { get; set; }
        public decimal BaseCost { get; set; }
        public decimal FinalAmount { get; set; }
        public string StrategyApplied { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    /// <summary>
    /// Billing Service with delegate-based strategies
    /// </summary>
    public static class BillingService
    {
        // DELEGATES with LAMBDA EXPRESSIONS
        public static readonly BillingStrategy StandardBilling = cost => cost;
        public static readonly BillingStrategy InsuredBilling = cost => cost * 0.20m;      // 80% covered
        public static readonly BillingStrategy SeniorCitizenBilling = cost => cost * 0.70m; // 30% discount
        public static readonly BillingStrategy GovernmentBilling = cost => cost * 0.10m;    // 90% subsidy

        public static BillingStrategy GetStrategy(int choice)
        {
            return choice switch
            {
                2 => InsuredBilling,
                3 => SeniorCitizenBilling,
                4 => GovernmentBilling,
                _ => StandardBilling
            };
        }

        public static string GetStrategyName(int choice)
        {
            return choice switch
            {
                2 => "Insured (80% coverage)",
                3 => "Senior Citizen (30% discount)",
                4 => "Government (90% subsidy)",
                _ => "Standard (No discount)"
            };
        }
    }

    #endregion

    #region ═══════════════ EVENTS (Hospital Notifications) ═══════════════

    /// <summary>
    /// Custom EventArgs for patient events
    /// </summary>
    public class PatientEventArgs : EventArgs
    {
        public Patient Patient { get; }
        public string Message { get; }

        public PatientEventArgs(Patient patient, string message)
        {
            Patient = patient;
            Message = message;
        }
    }

    /// <summary>
    /// Custom EventArgs for bill events
    /// </summary>
    public class BillEventArgs : EventArgs
    {
        public Bill Bill { get; }
        public BillEventArgs(Bill bill) { Bill = bill; }
    }

    /// <summary>
    /// EVENT PUBLISHER - Raises events for hospital activities
    /// </summary>
    public class HospitalEventManager
    {
        // EVENT declarations using EventHandler<T>
        public event EventHandler<PatientEventArgs> PatientAdmitted;
        public event EventHandler<BillEventArgs> BillGenerated;
        public event EventHandler<PatientEventArgs> EmergencyAlert;

        public void OnPatientAdmitted(Patient patient)
        {
            PatientAdmitted?.Invoke(this, new PatientEventArgs(patient, "Patient admitted"));
        }

        public void OnBillGenerated(Bill bill)
        {
            BillGenerated?.Invoke(this, new BillEventArgs(bill));
        }

        public void OnEmergencyAlert(Patient patient, string message)
        {
            EmergencyAlert?.Invoke(this, new PatientEventArgs(patient, message));
        }
    }

    /// <summary>
    /// EVENT SUBSCRIBERS - Department notification handlers
    /// </summary>
    public static class DepartmentNotifications
    {
        public static void ReceptionHandler(object sender, PatientEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  [RECEPTION] New admission: {e.Patient.Name}");
            Console.ResetColor();
        }

        public static void NursingHandler(object sender, PatientEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  [NURSING] Bed prepared for: {e.Patient.Name}");
            Console.ResetColor();
        }

        public static void PharmacyHandler(object sender, PatientEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  [PHARMACY] Medication requested for: {e.Patient.Name}");
            Console.ResetColor();
        }

        public static void AccountsHandler(object sender, BillEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"  [ACCOUNTS] Bill generated - Rs.{e.Bill.FinalAmount:N2}");
            Console.ResetColor();
        }

        public static void EmergencyHandler(object sender, PatientEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  [EMERGENCY] ALERT: {e.Message}");
            Console.ResetColor();
        }
    }

    #endregion

    #region ═══════════════ MAIN HOSPITAL SYSTEM ═══════════════

    /// <summary>
    /// Main Hospital System - Composition of all components
    /// </summary>
    public class HospitalSystem
    {
        private List<Patient> patients = new List<Patient>();
        private List<Bill> bills = new List<Bill>();
        private HospitalEventManager eventManager = new HospitalEventManager();
        private int patientCounter = 0;

        public HospitalSystem()
        {
            // Subscribe departments to events
            eventManager.PatientAdmitted += DepartmentNotifications.ReceptionHandler;
            eventManager.PatientAdmitted += DepartmentNotifications.NursingHandler;
            eventManager.PatientAdmitted += DepartmentNotifications.PharmacyHandler;
            eventManager.BillGenerated += DepartmentNotifications.AccountsHandler;
            eventManager.EmergencyAlert += DepartmentNotifications.EmergencyHandler;
        }

        public Patient AdmitPatient(int type, string name, int age, object[] extra)
        {
            string id = $"P{++patientCounter:D4}";

            Patient patient = type switch
            {
                1 => new GeneralPatient(id, name, age, extra[0].ToString()),
                2 => new EmergencyPatient(id, name, age, extra[0].ToString(), Convert.ToInt32(extra[1])),
                3 => new ICUPatient(id, name, age, Convert.ToInt32(extra[0]), Convert.ToBoolean(extra[1])),
                _ => throw new ArgumentException("Invalid type")
            };

            patients.Add(patient);
            Console.WriteLine("\n  Department Notifications:");
            eventManager.OnPatientAdmitted(patient);

            // Emergency alert for critical cases
            if (patient is EmergencyPatient ep && ep.SeverityLevel >= 4)
                eventManager.OnEmergencyAlert(patient, $"Critical - Severity {ep.SeverityLevel}/5");
            if (patient is ICUPatient icu && icu.RequiresVentilator)
                eventManager.OnEmergencyAlert(patient, "Ventilator support needed!");

            return patient;
        }

        public Bill GenerateBill(Patient patient, int strategyChoice)
        {
            BillingStrategy strategy = BillingService.GetStrategy(strategyChoice);
            decimal baseCost = patient.GetBaseTreatmentCost();

            Bill bill = new Bill
            {
                BillId = $"BILL-{DateTime.Now:yyyyMMddHHmmss}",
                Patient = patient,
                BaseCost = baseCost,
                FinalAmount = strategy(baseCost),  // DELEGATE invocation
                StrategyApplied = BillingService.GetStrategyName(strategyChoice),
                GeneratedAt = DateTime.Now
            };

            bills.Add(bill);
            Console.WriteLine("\n  Department Notifications:");
            eventManager.OnBillGenerated(bill);
            return bill;
        }

        public List<Patient> GetPatients() => patients;
        public List<Bill> GetBills() => bills;
    }

    #endregion

    #region ═══════════════ MAIN PROGRAM ═══════════════

    class Program
    {
        static HospitalSystem hospital = new HospitalSystem();

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n  ╔════════════════════════════════════════════╗");
                Console.WriteLine("  ║   HOSPITAL PATIENT MANAGEMENT SYSTEM       ║");
                Console.WriteLine("  ╚════════════════════════════════════════════╝");
                Console.ResetColor();

                Console.WriteLine("\n  1. Admit Patient");
                Console.WriteLine("  2. View Patients");
                Console.WriteLine("  3. Generate Bill");
                Console.WriteLine("  4. View Bills");
                Console.WriteLine("  5. Exit");
                Console.Write("\n  Enter choice: ");

                switch (Console.ReadLine())
                {
                    case "1": AdmitPatient(); break;
                    case "2": ViewPatients(); break;
                    case "3": GenerateBill(); break;
                    case "4": ViewBills(); break;
                    case "5": running = false; break;
                }
            }

            Console.WriteLine("\n  Goodbye!");
        }

        static void AdmitPatient()
        {
            Console.WriteLine("\n  Patient Type: 1.General  2.Emergency  3.ICU");
            Console.Write("  Choice: ");
            int type = int.Parse(Console.ReadLine() ?? "1");

            Console.Write("  Name: ");
            string name = Console.ReadLine() ?? "Unknown";

            Console.Write("  Age: ");
            int age = int.Parse(Console.ReadLine() ?? "30");

            object[] extra;
            switch (type)
            {
                case 1:
                    Console.Write("  Diagnosis: ");
                    extra = new object[] { Console.ReadLine() };
                    break;
                case 2:
                    Console.Write("  Emergency Type: ");
                    string et = Console.ReadLine();
                    Console.Write("  Severity (1-5): ");
                    int sev = int.Parse(Console.ReadLine() ?? "3");
                    extra = new object[] { et, sev };
                    break;
                case 3:
                    Console.Write("  ICU Days: ");
                    int days = int.Parse(Console.ReadLine() ?? "1");
                    Console.Write("  Ventilator (y/n): ");
                    bool vent = Console.ReadLine()?.ToLower() == "y";
                    extra = new object[] { days, vent };
                    break;
                default:
                    extra = new object[] { "General Checkup" };
                    break;
            }

            var patient = hospital.AdmitPatient(type, name, age, extra);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  Patient Admitted: {patient.GetPatientInfo()}");
            Console.ResetColor();
            Console.Write("\n  Press any key...");
            Console.ReadKey();
        }

        static void ViewPatients()
        {
            Console.WriteLine("\n  === ALL PATIENTS ===\n");
            var patients = hospital.GetPatients();
            if (patients.Count == 0)
            {
                Console.WriteLine("  No patients yet.");
            }
            else
            {
                foreach (var p in patients)
                {
                    Console.WriteLine($"  {p.GetPatientInfo()}");
                    Console.WriteLine($"    Base Cost: Rs.{p.GetBaseTreatmentCost():N2}\n");
                }
            }
            Console.Write("  Press any key...");
            Console.ReadKey();
        }

        static void GenerateBill()
        {
            var patients = hospital.GetPatients();
            if (patients.Count == 0)
            {
                Console.WriteLine("\n  No patients to bill.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n  Patients:");
            for (int i = 0; i < patients.Count; i++)
                Console.WriteLine($"  {i + 1}. {patients[i].Name} ({patients[i].PatientType})");

            Console.Write("  Select patient: ");
            int idx = int.Parse(Console.ReadLine() ?? "1") - 1;

            Console.WriteLine("\n  Billing Strategy:");
            Console.WriteLine("  1. Standard (No discount)");
            Console.WriteLine("  2. Insured (80% coverage)");
            Console.WriteLine("  3. Senior Citizen (30% discount)");
            Console.WriteLine("  4. Government (90% subsidy)");
            Console.Write("  Choice: ");
            int strategy = int.Parse(Console.ReadLine() ?? "1");

            var bill = hospital.GenerateBill(patients[idx], strategy);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  ═══════════════════════════════════════");
            Console.WriteLine($"  BILL: {bill.BillId}");
            Console.WriteLine($"  Patient: {bill.Patient.Name}");
            Console.WriteLine($"  Base Cost: Rs.{bill.BaseCost:N2}");
            Console.WriteLine($"  Strategy: {bill.StrategyApplied}");
            Console.WriteLine($"  FINAL: Rs.{bill.FinalAmount:N2}");
            Console.WriteLine($"  ═══════════════════════════════════════");
            Console.ResetColor();

            Console.Write("\n  Press any key...");
            Console.ReadKey();
        }

        static void ViewBills()
        {
            Console.WriteLine("\n  === ALL BILLS ===\n");
            var bills = hospital.GetBills();
            if (bills.Count == 0)
            {
                Console.WriteLine("  No bills yet.");
            }
            else
            {
                foreach (var b in bills)
                {
                    Console.WriteLine($"  {b.BillId} | {b.Patient.Name} | Rs.{b.FinalAmount:N2}");
                }
            }
            Console.Write("\n  Press any key...");
            Console.ReadKey();
        }
    }

    #endregion
}
