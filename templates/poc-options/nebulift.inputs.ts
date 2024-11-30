export default class Inputs {
  private macros: Record<string, any> = {}; // Meant to replace placeholders in the source code
  private constants: Record<string, any> = {}; // Meant to be used as constants in the template's pulumi code
  private env: Record<string, any> = {};

  constructor() {
    const rawInputs = process.env.NEBULIFT_INPUTS;
    if (!rawInputs) throw new Error("NEBULIFT_INPUTS env var is required");
    const inputs = JSON.parse(rawInputs);

	// inputs should be of type { macros: Record<string, any>, constants: Record<string, any>, env: Record<string, any> }
	// If this code is kept, we might want to add a check for the type of inputs
	console.log(inputs);
    this.macros = inputs.macros;
    this.constants = inputs.constants;
    this.env = inputs.env;
  }

  getMacro(name: string): any {
    if (this.macros[name] === undefined) throw new Error(`Macro ${name} not found`);
    return this.macros[name];
  }

  getMacroKeys(): string[] {
    return Object.keys(this.macros);
  }

  getConstant(name: string): any {
	if (this.constants[name] === undefined) throw new Error(`Constant ${name} not found`);
    return this.constants[name];
  }

  getEnv(name: string): any {
    if (this.env[name] == undefined) throw new Error(`Env var ${name} not found`);
    return this.env[name];
  }
}
