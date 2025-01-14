
export interface FieldConfig {
    value: string | string[];
    description: string;
  }
  
  export interface TechnologySectionSchema {  // Renamed from TechnologySection
    [fieldKey: string]: FieldConfig;
  }
  
  export interface TemplateSchema {
    [technology: string]: TechnologySectionSchema;
  }
  
  export interface FormData {
    [technology: string]: Record<string, string>;
  }