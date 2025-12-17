# Hospital Management System - Study Guide
## Complete Reference for Viva/Interview Questions

---

# PART 1: PROJECT OVERVIEW

## What is this project?
A **console-based Hospital Patient Management System** that manages:
- Different patient types (General, Emergency, ICU)
- Dynamic billing using **delegates**
- Real-time department notifications using **events**

## System Flow
```
Admit Patient → Select Type → Calculate Cost → Apply Billing Strategy → Generate Bill → Notify Departments
```

---

# PART 2: OOP CONCEPTS

## 1. ABSTRACTION

**Definition:** Hiding complex implementation details, showing only essential features.

**In Code:**
```csharp
public abstract class Patient
{
    public abstract decimal GetBaseTreatmentCost();  // No implementation here
}
```

**Why Abstract?**
- We can't create a generic "Patient" object
- Every patient MUST be a specific type (General/Emergency/ICU)
- Each type calculates cost differently

**Interview Answer:**
> "I used an abstract class because a Patient without a type doesn't make sense. The `GetBaseTreatmentCost()` is abstract because each patient type has different cost calculation logic."

---

## 2. ENCAPSULATION

**Definition:** Bundling data with methods, hiding internal details, controlling access.

**In Code:**
```csharp
public string PatientId { get; }           // Read-only (can't change after creation)
public string Name { get; set; }           // Read-write
private List<Patient> patients = new();    // Private field
```

**Why Encapsulation?**
- `PatientId` cannot be changed after patient is created (data integrity)
- `patients` list is private - external code can't directly modify it
- Access is controlled through methods like `GetPatients()`

**Interview Answer:**
> "PatientId is read-only because once assigned, it shouldn't change. The patients list is private to prevent external code from directly adding/removing patients without validation."

---

## 3. INHERITANCE

**Definition:** Creating new classes from existing classes, reusing code.

**In Code:**
```csharp
public abstract class Patient { }                    // Base class

public class GeneralPatient : Patient { }            // Derived class
public class EmergencyPatient : Patient { }          // Derived class  
public class ICUPatient : Patient { }                // Derived class
```

**Class Hierarchy:**
```
        Patient (Abstract Base)
             │
    ┌────────┼────────┐
    ↓        ↓        ↓
 General  Emergency   ICU
 Patient   Patient  Patient
```

**Why Inheritance?**
- **Code Reuse:** Common properties (Name, Age, PatientId) are in base class
- **Type Hierarchy:** All derived classes "IS-A" Patient
- **Single Inheritance:** C# allows only one base class (unlike interfaces)

**Interview Answer:**
> "I used inheritance because GeneralPatient, EmergencyPatient, and ICUPatient share common properties like Name, Age, and PatientId. Instead of duplicating code, I put common code in the base Patient class."

---

## 4. POLYMORPHISM

**Definition:** Same method name, different behavior based on object type.

**In Code:**
```csharp
// In GeneralPatient
public override decimal GetBaseTreatmentCost() => 5000m;

// In EmergencyPatient  
public override decimal GetBaseTreatmentCost() => 15000m + (SeverityLevel * 2000m);

// In ICUPatient
public override decimal GetBaseTreatmentCost() 
{
    decimal cost = 25000m + (ICUDays * 8000m);
    return RequiresVentilator ? cost + 15000m : cost;
}
```

**Runtime Polymorphism Example:**
```csharp
Patient patient = new EmergencyPatient(...);  // Declared as Patient
patient.GetBaseTreatmentCost();               // Calls EmergencyPatient's method!
```

**Types of Polymorphism:**
1. **Compile-time (Static):** Method Overloading (same name, different parameters)
2. **Runtime (Dynamic):** Method Overriding (same name, different implementation in derived class)

**Interview Answer:**
> "Polymorphism allows me to call `GetBaseTreatmentCost()` on any Patient object, and the correct method executes based on actual object type at runtime. This is runtime polymorphism achieved through method overriding."

---

# PART 3: DELEGATES

## What is a Delegate?

**Definition:** A type-safe function pointer that holds reference to methods with a specific signature.

**In Code:**
```csharp
// Delegate declaration - defines the method signature
public delegate decimal BillingStrategy(decimal baseCost);

// Creating delegate instances using lambda expressions
public static readonly BillingStrategy StandardBilling = cost => cost;
public static readonly BillingStrategy InsuredBilling = cost => cost * 0.20m;
public static readonly BillingStrategy SeniorCitizenBilling = cost => cost * 0.70m;
```

**Using the Delegate:**
```csharp
BillingStrategy strategy = InsuredBilling;      // Assign method to delegate
decimal finalAmount = strategy(baseCost);       // Invoke delegate like a method
```

**Why Delegates?**
- **Flexibility:** Change billing logic at runtime without modifying code
- **Strategy Pattern:** Different algorithms (strategies) can be swapped
- **Callback Mechanism:** Pass methods as parameters

**Interview Answer:**
> "I used delegates for billing because different patients may have different discount schemes. Instead of writing if-else conditions, I use delegates to dynamically select and apply the billing strategy at runtime."

---

# PART 4: EVENTS

## What is an Event?

**Definition:** A notification mechanism that allows objects to communicate when something happens.

## Publisher-Subscriber Pattern

```
┌─────────────────────────┐
│  HospitalEventManager   │  ← PUBLISHER (raises events)
│  (Event Publisher)      │
├─────────────────────────┤
│  PatientAdmitted event  │──────┐
│  BillGenerated event    │──────┼──→  SUBSCRIBERS (handle events)
│  EmergencyAlert event   │──────┘
└─────────────────────────┘
         │
         ↓
┌─────────────────────────┐
│  Department Handlers    │  ← SUBSCRIBERS
├─────────────────────────┤
│  Reception Handler      │
│  Nursing Handler        │
│  Pharmacy Handler       │
│  Accounts Handler       │
│  Emergency Handler      │
└─────────────────────────┘
```

## Code Breakdown

**1. Event Declaration (Publisher):**
```csharp
public event EventHandler<PatientEventArgs> PatientAdmitted;
```

**2. Custom EventArgs (Data passed with event):**
```csharp
public class PatientEventArgs : EventArgs
{
    public Patient Patient { get; }
    public string Message { get; }
}
```

**3. Raising Event:**
```csharp
public void OnPatientAdmitted(Patient patient)
{
    PatientAdmitted?.Invoke(this, new PatientEventArgs(patient, "Admitted"));
}
```

**4. Subscribing to Event:**
```csharp
eventManager.PatientAdmitted += DepartmentNotifications.ReceptionHandler;
eventManager.PatientAdmitted += DepartmentNotifications.NursingHandler;
```

**5. Event Handler (Subscriber):**
```csharp
public static void ReceptionHandler(object sender, PatientEventArgs e)
{
    Console.WriteLine($"New admission: {e.Patient.Name}");
}
```

## Events vs Delegates

| Aspect | Delegate | Event |
|--------|----------|-------|
| Invocation | Can be invoked from anywhere | Only from declaring class |
| Assignment | Can be reassigned (=) | Only += or -= allowed |
| Purpose | General method reference | Notification mechanism |
| Encapsulation | Less restricted | More encapsulated |

**Interview Answer:**
> "I used events for department notifications because when a patient is admitted, multiple departments need to be notified. Events allow loose coupling - the HospitalSystem doesn't need to know which departments exist. New departments can subscribe without changing existing code."

---

# PART 5: OTHER C# CONCEPTS

## Lambda Expressions
```csharp
// Traditional method
decimal Calculate(decimal cost) { return cost * 0.20m; }

// Lambda expression (same thing, shorter)
BillingStrategy insured = cost => cost * 0.20m;
```

**Interview Answer:**
> "Lambda expressions are anonymous functions. I used them to define billing strategies concisely without creating separate methods."

## Switch Expressions (C# 8+)
```csharp
// Traditional switch
switch(type) {
    case 1: return new GeneralPatient(...);
    case 2: return new EmergencyPatient(...);
    default: throw new Exception();
}

// Switch expression (shorter)
Patient patient = type switch
{
    1 => new GeneralPatient(...),
    2 => new EmergencyPatient(...),
    _ => throw new Exception()
};
```

## Null Conditional Operator (?.)
```csharp
PatientAdmitted?.Invoke(this, args);
// Only invokes if PatientAdmitted is not null
```

---

# PART 6: COMMON VIVA QUESTIONS

## Q1: Why abstract class and not interface?
**A:** Abstract class can have:
- Implemented methods (like `GetPatientInfo()`)
- Fields and properties with values
- Constructors
Interface only has method signatures. Since Patient has common implemented code, abstract class is better.

## Q2: Can we create an object of abstract class?
**A:** No. Abstract classes cannot be instantiated directly. We must create objects of derived classes.

## Q3: What is the difference between override and new keyword?
**A:** 
- `override`: Replaces base method, polymorphism works
- `new`: Hides base method, polymorphism doesn't work

## Q4: Why use events instead of direct method calls?
**A:** 
- **Loose Coupling:** Publisher doesn't know subscribers
- **Multiple Subscribers:** Many handlers for one event
- **Extensibility:** Add new subscribers without changing publisher

## Q5: What is EventHandler<T>?
**A:** A predefined delegate type: `void EventHandler<T>(object sender, T args)` where T is EventArgs.

## Q6: What happens if no one subscribes to an event?
**A:** Event is null. That's why we use `?.Invoke()` - it only invokes if not null.

## Q7: Explain the flow when a patient is admitted.
**A:**
1. User enters patient details
2. `AdmitPatient()` creates appropriate patient object (Factory pattern)
3. Patient added to list
4. `OnPatientAdmitted()` raises event
5. All subscribed department handlers execute
6. If critical (severity ≥4 or ventilator needed), emergency alert raised

## Q8: How does billing strategy work?
**A:**
1. User selects a strategy (Standard/Insured/Senior/Government)
2. Corresponding delegate is assigned to `BillingStrategy` variable
3. Delegate is invoked: `finalAmount = strategy(baseCost)`
4. Each strategy applies different calculation
5. Bill generated and event raised

---

# PART 7: CODE SUMMARY

| Line Range | What It Contains |
|------------|------------------|
| 1-110 | **MODELS** - Patient classes (OOP concepts) |
| 112-165 | **DELEGATES** - BillingStrategy and BillingService |
| 167-245 | **EVENTS** - EventManager and Department handlers |
| 247-310 | **MAIN SYSTEM** - HospitalSystem class |
| 312-400 | **MAIN PROGRAM** - Console UI and menu |

---

**Good Luck with your Viva! 🎓**
