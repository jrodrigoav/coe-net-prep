import { createRoot } from "react-dom/client";
import { App } from "./App";

const container = document.getElementById("frontend-app");
const root = createRoot(container)
root.render(<App />);