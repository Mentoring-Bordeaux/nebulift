
export interface FieldConfig {
    type: string;
    title: string;
    description: string;
    enum?: string[]
}

export interface SectionSchema {  // Renamed from TechnologySection
    type: string;
    title: string;
    description: string;
    properties: {
        [fieldKey: string]: FieldConfig;
    };
}

export interface TemplateSchema {
    [sectionKey: string]: SectionSchema;
}

export interface FormData {
    [sectionKey: string]: Record<string, string>;
}