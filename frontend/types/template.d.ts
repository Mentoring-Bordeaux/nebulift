
export interface FieldConfig {
    type: string;
    title: string;
    description: string;
    enum?: string[]
    minItems?: number;
    items?: {
        type: string;
        title: string;
        uniqueItems?: boolean;
    };
}

export interface SectionSchema {  // Corresponds to elements of type 'object' (first layer) of the inputs json schema 
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
    [sectionKey: string]: Record<string, string | string[]>;
}