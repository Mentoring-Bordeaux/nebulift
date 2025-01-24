import * as github from "@pulumi/github";
import * as fs from "fs";
import Inputs from "./nebulift.inputs";

export { Inputs };

const firstCommitMessage = "Add source code";

let inputs: Inputs | null = null;

export function init(): Inputs {
  return (inputs = new Inputs());
}

// Parse every file in the source code directory and add them to the repository
// NOTE: This function does not handle .gitignore yet
export function addSourceCode(
  repositoryName: string,
  sourcePath: string,
  macros: Record<string, string> = {}
): void {
  function readAndReplace(filePath: string): string {
    if (inputs == null) throw new Error("Inputs not initialized");

    let fileContent = fs.readFileSync(filePath, "utf8");

    // Replace all macros in the file
    for (const macro in macros) {
      const regex = new RegExp(`@@@${macro}@@@`, "g");
      fileContent = fileContent.replace(regex, macros[macro]);
    }
    // TODO: Complete with @@@for (...)@@@, @@@if (...)@@@, etc without forgetting nested ones

    return fileContent;
  }

  function addFile(filePath: string): void {
    const file = filePath.split("/").pop();
    if (!file) throw new Error("Invalid file path");
    new github.RepositoryFile(filePath, {
      repository: repositoryName,
      file: filePath.slice(sourcePath.length + 1),
      content: readAndReplace(filePath),
      commitMessage: firstCommitMessage
    });
  }

  function walkSync(dir: string) {
    fs.readdirSync(dir).forEach((file) => {
      const path = `${dir}/${file}`;
      fs.statSync(path).isDirectory() ? walkSync(path) : addFile(path);
    });
  }

  walkSync(sourcePath);
}
