export default class Inputs {
  private blocks: Record<string, Record<string, any>> = {};

  constructor() {
    const rawInputs = process.env.NEBULIFT_INPUTS;
    if (!rawInputs) throw new Error("NEBULIFT_INPUTS environment variable is required");
    const inputs = JSON.parse(rawInputs);

    for (const key in inputs) {
      this.blocks[key] = inputs[key];
    }
  }

  getSectionDict(section: string): Record<string, any> {
    if (this.blocks[section] === undefined) throw new Error(`Macro ${section} not found`);
    return this.blocks[section];
  }

  getSectionValue(section: string, value: string): any {
    if (this.blocks[section] === undefined) throw new Error(`Block ${section} not found`);
    if (this.blocks[section][value] === undefined) throw new Error(`Value ${value} not found in block ${section}`);
    return this.blocks[section][value];
  }
}
