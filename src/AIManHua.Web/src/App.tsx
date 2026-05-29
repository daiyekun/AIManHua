import { Routes, Route } from "react-router-dom";
import Layout from "./components/Layout/Layout";
import ComicCreatePage from "./pages/ComicCreatePage";
import ComicEditPage from "./pages/ComicEditPage";
import HistoryPage from "./pages/HistoryPage";

export default function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<ComicCreatePage />} />
        <Route path="/edit/:taskId" element={<ComicEditPage />} />
        <Route path="/history" element={<HistoryPage />} />
      </Route>
    </Routes>
  );
}
