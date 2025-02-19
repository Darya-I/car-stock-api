import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'
import { ChakraProvider, defaultSystem } from '@chakra-ui/react'
import { AuthProvider } from './context/AuthProvider'

createRoot(document.getElementById('root')).render(
  <AuthProvider>
    <ChakraProvider value={defaultSystem} >
      <App />
    </ChakraProvider>
  </AuthProvider>
)
