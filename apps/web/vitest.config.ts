import { defineConfig, mergeConfig } from "vitest/config";
import viteConfig from "./vite.config.ts";

export default mergeConfig(
  viteConfig,
  defineConfig({
    test: {
      environment: "jsdom",
      setupFiles: ["./src/testing/setupTests.ts"],
      coverage: {
        reporter: ["lcov", "text"],
      },
    },
  }),
);
