import { useState } from "react";
import Login from "@/components/Login";
import Register from "@/components/Register";
import ToggleContainer from "@/components/ToggleContainer";

const AuthPage = () => {
  const [isActive, setIsActive] = useState(false);

  const toggleActive = () => {
    setIsActive((prevState) => !prevState);
  };

  return (
    <div className="flex items-center justify-center flex-col h-full bg-gradient-to-r from-[#e2e2e2] to-[#c9d6ff]">
      <div
        id="containerr"
        className="bg-[#fff] rounded-lg shadow-lg relative overflow-hidden w-[768px] max-w-full min-h-[480px]"
      >
        <Login isActiveContainer={isActive} />
        <Register isActiveContainer={isActive} />
        <ToggleContainer
          toggleActive={toggleActive}
          isActiveContainer={isActive}
        />
      </div>
    </div>
  );
};

export default AuthPage;
