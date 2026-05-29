import { useParams } from "react-router-dom";
import StoryboardPreview from "../components/StoryboardPreview/StoryboardPreview";
import CanvasEditor from "../components/CanvasEditor/CanvasEditor";
import DialogueEditor from "../components/DialogueEditor/DialogueEditor";

export default function ComicEditPage() {
  const { taskId } = useParams<{ taskId: string }>();

  return (
    <div className="page">
      <h2>编辑漫画 #{taskId}</h2>
      <div className="edit-layout">
        <StoryboardPreview />
        <CanvasEditor />
        <DialogueEditor />
      </div>
    </div>
  );
}
