import { z } from "zod";

const envSchema = z.object({
  VITE_API_URL: z.url(),
});

const parsedEnv = envSchema.parse(import.meta.env);

export const env = {
  apiUrl: parsedEnv.VITE_API_URL,
};
