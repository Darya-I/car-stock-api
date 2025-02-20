import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import jsconfigPaths from "vite-jsconfig-paths"

// https://vite.dev/config/
export default defineConfig({
  server: { 
    https: {
      key: './carstock-privateKey.key',
      cert: './carstock.crt',
    },
    hmr: true
  },
  plugins: [react(), jsconfigPaths()]
})
