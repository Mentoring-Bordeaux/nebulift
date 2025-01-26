export const formatExecutionDuration = (startTime: number): string => {
    const duration = Date.now() - startTime;
    return duration < 1000 
      ? `${duration}ms` 
      : `${(duration / 1000).toFixed(1)}s`;
  };
  
  export const isValidOutput = (output: unknown): boolean => {
    if (!output || typeof output !== 'object') return false;
    
    return Object.entries(output as Record<string, unknown>).every(([_, field]) => {
      return field && typeof field === 'object' &&
        'description' in field &&
        'title' in field &&
        'type' in field &&
        'value' in field;
    });
  };