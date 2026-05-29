export default function PromptInput() {
  return (
    <div className="prompt-input">
      <textarea
        placeholder="请输入漫画创作的提示词，描述场景、角色和剧情..."
        rows={6}
      />
      <div className="prompt-options">
        <select defaultValue="manga">
          <option value="manga">日漫风格</option>
          <option value="manhua">国漫风格</option>
          <option value="webtoon">韩漫风格</option>
          <option value="american">美漫风格</option>
          <option value="watercolor">水彩风格</option>
        </select>
        <button type="button">生成漫画</button>
      </div>
    </div>
  );
}
