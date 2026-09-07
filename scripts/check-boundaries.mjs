import { existsSync, readdirSync, readFileSync, statSync } from "node:fs";
import { join, relative, sep } from "node:path";
import { fileURLToPath } from "node:url";

const root = join(fileURLToPath(new URL("..", import.meta.url)));
const violations = [];

if (!existsSync(join(root, "package.json"))) {
  violations.push("root package.json is missing");
}

const sourceExtensions = new Set([".js", ".jsx", ".mjs", ".cjs", ".ts", ".tsx"]);
const ignoredDirectories = new Set(["node_modules", ".git", "dist", "build"]);

function sourceFiles(directory) {
  if (!existsSync(directory)) {
    return [];
  }

  const files = [];
  for (const entry of readdirSync(directory, { withFileTypes: true })) {
    if (ignoredDirectories.has(entry.name)) {
      continue;
    }

    const path = join(directory, entry.name);
    if (entry.isDirectory()) {
      files.push(...sourceFiles(path));
    } else if (sourceExtensions.has(entry.name.slice(entry.name.lastIndexOf(".")))) {
      files.push(path);
    }
  }
  return files;
}

function importsServiceInternals(source) {
  return /(?:from|import|require\s*\()\s*["'][^"']*(?:services[\\/]|@coffee-pos\/(?:api|worker))/u.test(source);
}

function importsApplications(source) {
  return /(?:from|import|require\s*\()\s*["'][^"']*(?:[\\/]apps[\\/]|@coffee-pos\/(?:pos|admin|kds))/u.test(source);
}

for (const packageRoot of ["packages/contracts", "packages/ui"]) {
  for (const file of sourceFiles(join(root, packageRoot))) {
    const source = readFileSync(file, "utf8");
    if (importsApplications(source)) {
      violations.push(`${relative(root, file)} imports an application boundary`);
    }
  }
}

for (const appRoot of ["apps/pos", "apps/admin", "apps/kds"]) {
  for (const file of sourceFiles(join(root, appRoot))) {
    const source = readFileSync(file, "utf8");
    if (importsServiceInternals(source)) {
      violations.push(`${relative(root, file)} imports a service boundary`);
    }
  }
}

if (violations.length > 0) {
  console.error("Boundary checks failed:");
  for (const violation of violations) {
    console.error(`- ${violation}`);
  }
  process.exitCode = 1;
} else {
  console.log("Boundary checks passed: packages do not import apps and apps do not import service internals.");
}
