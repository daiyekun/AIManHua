import { useState, type FormEvent } from "react";
import "./AuthModal.css";

interface Props {
  isOpen: boolean;
  onClose: () => void;
  initialTab?: "login" | "register";
}

export default function AuthModal({ isOpen, onClose, initialTab = "login" }: Props) {
  const [tab, setTab] = useState<"login" | "register">(initialTab);
  const [showPwd, setShowPwd] = useState(false);
  const [agreed, setAgreed] = useState(false);

  if (!isOpen) return null;

  const switchTab = (t: "login" | "register") => { setTab(t); setShowPwd(false); setAgreed(false); };

  const handleLogin = (e: FormEvent) => { e.preventDefault(); onClose(); };
  const handleRegister = (e: FormEvent) => { e.preventDefault(); onClose(); };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-card" onClick={(e) => e.stopPropagation()}>
        <button className="modal-close" onClick={onClose}>&#10005;</button>

        <div className="modal-tabs">
          <button className={`modal-tab ${tab === "login" ? "active" : ""}`} onClick={() => switchTab("login")}>登录</button>
          <button className={`modal-tab ${tab === "register" ? "active" : ""}`} onClick={() => switchTab("register")}>注册</button>
        </div>

        {tab === "login" ? (
          <form onSubmit={handleLogin} className="modal-form">
            <label className="field">
              <span>账号 / 邮箱</span>
              <input type="text" placeholder="请输入邮箱或用户名" required />
            </label>
            <label className="field">
              <span>密码</span>
              <div className="pwd-wrap">
                <input type={showPwd ? "text" : "password"} placeholder="请输入密码" required />
                <button type="button" className="pwd-toggle" onClick={() => setShowPwd(!showPwd)}>
                  {showPwd ? "🙈" : "👁"}
                </button>
              </div>
            </label>
            <div className="form-aux">
              <label className="checkbox-label"><input type="checkbox" /> 记住密码</label>
              <a href="#">忘记密码？</a>
            </div>
            <button type="submit" className="btn btn-primary btn-lg modal-submit">登录</button>
            <p className="form-switch">还没有账号？<button type="button" className="link-btn" onClick={() => switchTab("register")}>去注册</button></p>
          </form>
        ) : (
          <form onSubmit={handleRegister} className="modal-form">
            <label className="field">
              <span>用户名</span>
              <input type="text" placeholder="请输入用户名" required />
            </label>
            <label className="field">
              <span>邮箱</span>
              <input type="email" placeholder="请输入邮箱" required />
            </label>
            <label className="field">
              <span>密码</span>
              <div className="pwd-wrap">
                <input type={showPwd ? "text" : "password"} placeholder="至少6位密码" required minLength={6} />
                <button type="button" className="pwd-toggle" onClick={() => setShowPwd(!showPwd)}>
                  {showPwd ? "🙈" : "👁"}
                </button>
              </div>
            </label>
            <label className="field">
              <span>确认密码</span>
              <input type="password" placeholder="再次输入密码" required minLength={6} />
            </label>
            <label className="checkbox-label">
              <input type="checkbox" checked={agreed} onChange={(e) => setAgreed(e.target.checked)} />
              已阅读并同意<a href="#">《用户协议》</a><a href="#">《隐私政策》</a>
            </label>
            <button type="submit" className="btn btn-primary btn-lg modal-submit" disabled={!agreed}>立即注册</button>
            <p className="form-switch">已有账号？<button type="button" className="link-btn" onClick={() => switchTab("login")}>去登录</button></p>
          </form>
        )}
      </div>
    </div>
  );
}
