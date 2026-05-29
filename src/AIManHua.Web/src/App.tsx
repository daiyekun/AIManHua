import { Routes, Route } from "react-router-dom";
import HomePage from "./pages/HomePage";
import HistoryPage from "./pages/HistoryPage";
import ComicEditPage from "./pages/ComicEditPage";

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/edit/:taskId" element={<ComicEditPage />} />
      <Route path="/history" element={<HistoryPage />} />
    </Routes>
  );
}
