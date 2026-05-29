import { useState, type FormEvent, type ChangeEvent } from "react";
import { registerApi, loginApi } from "../../services/api";
import { useAuthStore } from "../../store/useAuthStore";
import "./AuthModal.css";

interface Props {
  isOpen: boolean;
  onClose: () => void;
  initialTab?: "login" | "register";
}

interface FormFields {
  username: string;
  email: string;
  password: string;
  confirmPwd: string;
}

const emptyFields: FormFields = { username: "", email: "", password: "", confirmPwd: "" };

export default function AuthModal({ isOpen, onClose, initialTab = "login" }: Props) {
  const setAuth = useAuthStore((s) => s.setAuth);
  const [tab, setTab] = useState<"login" | "register">(initialTab);
  const [fields, setFields] = useState<FormFields>(emptyFields);
  const [showPwd, setShowPwd] = useState(false);
  const [agreed, setAgreed] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [fieldErrors, setFieldErrors] = useState<Partial<Record<keyof FormFields, string>>>({});

  if (!isOpen) return null;

  const update = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFields((prev) => ({ ...prev, [name]: value }));
    if (fieldErrors[name as keyof FormFields]) {
      setFieldErrors((prev) => ({ ...prev, [name]: undefined }));
    }
  };

  const reset = () => {
    setFields(emptyFields);
    setShowPwd(false);
    setAgreed(false);
    setError("");
    setSuccess("");
    setFieldErrors({});
  };

  const switchTab = (t: "login" | "register") => { setTab(t); reset(); };

  const validate = (): boolean => {
    const errs: Partial<Record<keyof FormFields, string>> = {};
    if (!fields.email.trim()) errs.email = "请输入邮箱";
    else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(fields.email)) errs.email = "邮箱格式不正确";
    if (!fields.password) errs.password = "请输入密码";
    else if (fields.password.length < 6) errs.password = "密码至少6位";

    if (tab === "register") {
      if (!fields.username.trim()) errs.username = "请输入用户名";
      else if (fields.username.trim().length < 2) errs.username = "用户名至少2个字符";
      if (fields.password !== fields.confirmPwd) errs.confirmPwd = "两次密码不一致";
      if (!agreed) setError("请先同意用户协议和隐私政策");
    }
    setFieldErrors(errs);
    return Object.keys(errs).length === 0 && (tab === "login" || agreed);
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError("");
    setSuccess("");
    if (!validate()) return;

    setLoading(true);
    try {
      const payload = { email: fields.email.trim(), password: fields.password, username: fields.username.trim() || undefined };
      const result = tab === "login" ? await loginApi(payload) : await registerApi(payload);

      setAuth(
        { userId: result.userId, username: result.username, email: result.email },
        result.accessToken,
        result.expiresAt
      );

      const msg = tab === "login" ? `欢迎回来，${result.username}！` : `注册成功，欢迎 ${result.username}！`;
      setSuccess(msg);
      setTimeout(() => onClose(), 1200);
    } catch (err: unknown) {
      setError(err instanceof Error ? err.message : "操作失败，请稍后重试");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-card" onClick={(e) => e.stopPropagation()}>
        <button className="modal-close" onClick={onClose}>&#10005;</button>

        <div className="modal-tabs">
          <button className={`modal-tab ${tab === "login" ? "active" : ""}`} onClick={() => switchTab("login")}>登录</button>
          <button className={`modal-tab ${tab === "register" ? "active" : ""}`} onClick={() => switchTab("register")}>注册</button>
        </div>

        {/* 全局错误 / 成功提示 */}
        {error && <div className="alert alert-error">{error}</div>}
        {success && <div className="alert alert-success">{success}</div>}

        {tab === "login" ? (
          <form onSubmit={handleSubmit} className="modal-form">
            <label className="field">
              <span>邮箱</span>
              <input name="email" type="text" value={fields.email} onChange={update} placeholder="请输入邮箱" />
              {fieldErrors.email && <span className="field-err">{fieldErrors.email}</span>}
            </label>
            <label className="field">
              <span>密码</span>
              <div className="pwd-wrap">
                <input name="password" type={showPwd ? "text" : "password"} value={fields.password} onChange={update} placeholder="请输入密码" />
                <button type="button" className="pwd-toggle" onClick={() => setShowPwd(!showPwd)}>{showPwd ? "🙈" : "👁"}</button>
              </div>
              {fieldErrors.password && <span className="field-err">{fieldErrors.password}</span>}
            </label>
            <button type="submit" className="btn btn-primary btn-lg modal-submit" disabled={loading}>
              {loading ? "登录中..." : "登录"}
            </button>
            <p className="form-switch">还没有账号？<button type="button" className="link-btn" onClick={() => switchTab("register")}>去注册</button></p>
          </form>
        ) : (
          <form onSubmit={handleSubmit} className="modal-form">
            <label className="field">
              <span>用户名</span>
              <input name="username" type="text" value={fields.username} onChange={update} placeholder="请输入用户名" />
              {fieldErrors.username && <span className="field-err">{fieldErrors.username}</span>}
            </label>
            <label className="field">
              <span>邮箱</span>
              <input name="email" type="text" value={fields.email} onChange={update} placeholder="请输入邮箱" />
              {fieldErrors.email && <span className="field-err">{fieldErrors.email}</span>}
            </label>
            <label className="field">
              <span>密码</span>
              <div className="pwd-wrap">
                <input name="password" type={showPwd ? "text" : "password"} value={fields.password} onChange={update} placeholder="至少6位密码" />
                <button type="button" className="pwd-toggle" onClick={() => setShowPwd(!showPwd)}>{showPwd ? "🙈" : "👁"}</button>
              </div>
              {fieldErrors.password && <span className="field-err">{fieldErrors.password}</span>}
            </label>
            <label className="field">
              <span>确认密码</span>
              <input name="confirmPwd" type="password" value={fields.confirmPwd} onChange={update} placeholder="再次输入密码" />
              {fieldErrors.confirmPwd && <span className="field-err">{fieldErrors.confirmPwd}</span>}
            </label>
            <label className="checkbox-label">
              <input type="checkbox" checked={agreed} onChange={(e) => setAgreed(e.target.checked)} />
              已阅读并同意<a href="#">《用户协议》</a><a href="#">《隐私政策》</a>
            </label>
            <button type="submit" className="btn btn-primary btn-lg modal-submit" disabled={loading || !agreed}>
              {loading ? "注册中..." : "立即注册"}
            </button>
            <p className="form-switch">已有账号？<button type="button" className="link-btn" onClick={() => switchTab("login")}>去登录</button></p>
          </form>
        )}
      </div>
    </div>
  );
}
