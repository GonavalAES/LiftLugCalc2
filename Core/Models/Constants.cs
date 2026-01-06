namespace LiftLugCalc2.Core.Models
{
    public static class Constants
    {
        // ========== UI DATE FORMAT ==========
        public const string DATE_FORMAT = "dd-MM-yyyy";

        // ========== DATA FILE NAMES ==========
        public const string FILE_LUGS = "LugType.csv";
        public const string FILE_MATERIALS = "Materials.csv";

        // ========== SAFETY FACTORS ==========
        public const double MINIMUM_SAFETY_FACTOR = 2.0;

        // ========== MATERIAL RESISTANCE FACTORS (γRm) ==========
        // Per NORSOK R-002 or similar standards
        public const double GAMMA_RM_STRUCTURAL = 1.15;     // For structural parts and full penetration welds
        public const double GAMMA_RM_FILLET = 1.3;          // For fillet welds, partial welds

        // ========== BEARING FACTOR ==========
        public const double BEARING_FACTOR = 1.5;           // Bearing factor for pin/hole contact

        // ========== PHYSICAL CONSTANTS ==========
        public const double GRAVITY = 9.81;                 // m/s² (for kg to N conversion)

        // ========== DYNAMIC FACTORS ==========
        public const double DAF = 1.3;                      // Dynamic Amplification Factor per NORSOK

        // ========== WEIGHT CORRECTION FACTORS (WCF) ==========
        public const double WCF_MEASURED = 1.03;            // Weighing / measured ±3%
        public const double WCF_DETAILED_UPDATED = 1.10;    // Detailed calc from updated drawings
        public const double WCF_DETAILED_OLDER = 1.20;      // Detailed calc from older/less accurate drawings
        public const double WCF_STANDARD = 1.30;            // Standard-mandated
        public const double WCF_DEFAULT = 1.20;             // Default safe value

        // ========== WELD GEOMETRY LIMITS ==========
        public const double WELD_THROAT_MAX_RATIO = 0.7;    // Max weld throat as ratio of plate thickness
        public const double WELD_THROAT_MIN = 3.0;          // Minimum weld throat thickness (mm)

        // ========== UNIT CONVERSIONS ==========
        public const double KG_TO_KN = GRAVITY / 1000.0;    // Convert kg to kN
        public const double KN_TO_KG = 1000.0 / GRAVITY;    // Convert kN to kg

        // ========== ANGLE LIMITS (degrees) ==========
        public const double MIN_SLING_ANGLE = 0.0;          // Vertical (from vertical)
        public const double MAX_SLING_ANGLE = 90.0;         // Horizontal (from vertical)
        public const double RECOMMENDED_MIN_ANGLE = 15.0;   // Recommended minimum working angle
        public const double RECOMMENDED_MAX_ANGLE = 45.0;   // Recommended maximum working angle
        public const double WARNING_ANGLE = 60.0;           // Large angle warning threshold

    }
}
