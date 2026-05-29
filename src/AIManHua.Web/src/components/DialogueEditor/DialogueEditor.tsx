export default function DialogueEditor() {
  return (
    <div className="dialogue-editor">
      <h3>台词修改</h3>
      <textarea placeholder="选择分镜面板后编辑台词..." rows={4} />
      <button type="button">应用台词</button>
    </div>
  );
}
