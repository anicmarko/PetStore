interface ToggleContainerProps {
  isActiveContainer: boolean;
  toggleActive: () => void;
}

const ToggleContainer = ({ isActiveContainer, toggleActive }: ToggleContainerProps) => {
  return (
    <div
      className={`absolute top-0 left-1/2 w-1/2 h-full overflow-hidden transition-all duration-500 ease-in-out z-[1000] ${
        isActiveContainer
          ? "translate-x-[-100%] rounded-tr-[150px] rounded-br-[100px] rounded-tl-none rounded-bl-none"
          : "rounded-tl-[150px] rounded-bl-[100px] rounded-tr-none rounded-br-none"
      }`}
      id="toggle-container"
    >
      <div
        id="toggle"
        className={`bg-[#512da8] h-full bg-gradient-to-r from-[#5c6bc0] to-[#512da8] text-[#fff] relative left-[-100%] w-[200%] transition-all duration-500 ease-in-out ${
          isActiveContainer ? "translate-x-[50%]" : "translate-x-0"
        }`}
      >
        <div
          id="toggle-panel-toggle-left"
          className={`absolute w-1/2 h-full flex items-center justify-center flex-col px-8 py-0 text-center top-0 transition-all duration-500 ease-in-out ${
            isActiveContainer ? "translate-x-0" : "translate-x-[-200%]"
          }`}
        >
          <h1 className="text-3xl font-semibold text-center mb-4 tracking-wider">Welcome Back!</h1>
          <p className="text-sm tracking-[0.3px] my-5 mx-0">Enter your personal details to use all of site features</p>
          <button
            className="bg-transparent border-[#fff] text-[#fff] text-xs py-2.5 px-11 rounded-md border border-solid font-semibold tracking-[0.5px] uppercase mt-2.5 pointer"
            id="login"
            onClick={toggleActive}
          >
            Sign In
          </button>
        </div>

        <div
          id="toggle-panel-toggle-right"
          className={`absolute w-1/2 h-full flex items-center justify-center flex-col px-8 py-0 text-center top-0 transition-all duration-500 ease-in-out ${
            isActiveContainer ? "translate-x-[200%]" : "translate-x-0"
          } right-0`}
        >
          <h1 className="text-3xl font-semibold text-center mb-4 tracking-wider">Hello, Friend!</h1>
          <p className="text-sm tracking-[0.3px] my-5 mx-0">Register with your personal details to use all of site features</p>
          <button
            className="bg-transparent border-[#fff] text-[#fff] text-xs py-2.5 px-11 rounded-md border border-solid font-semibold tracking-[0.5px] uppercase mt-2.5 pointer"
            id="register"
            onClick={toggleActive}
          >
            Sign Up
          </button>
        </div>
      </div>
    </div>
  );
};

export default ToggleContainer;
