import { Button } from "@/components/ui/button";
import { Link } from "react-router-dom";

import {
  Form,
  FormItem,
  FormControl,
  FormMessage,
} from "@/components/ui/form";
import { useForm, Controller } from "react-hook-form";

interface LoginProps {
  isActiveContainer: boolean;

}


const Login = ({isActiveContainer} : LoginProps) => {
  const form = useForm({
    defaultValues: {
      email: "",
      password: "",
    },
  });

  const onSubmit = (data: any) => {
    console.error(data);
  };

  return (
    <Form {...form}>
      <div className={`flex items-center justify-center absolute top-0 h-full transition-all duration-500 ease-in-out left-0 w-1/2 z-[2] ${isActiveContainer ? 'translate-x-[100%]' : ''} `}>
      <form
          onSubmit={form.handleSubmit(onSubmit)}
          className="flex flex-col w-full justify-center px-10 py-0 bg-[#fff] ">
          <h1 className="text-3xl font-semibold text-center mb-4 tracking-wider">Sign in</h1>
          
          <FormItem>
            <FormControl>
              <Controller
                name="email"
                control={form.control}
                render={({ field }) => (
                  <input
                    type="email"
                    placeholder="Email"
                    className="bg-[#eee] border-none my-2 mx-0 text-xs py-2.5 px-4 text rounded-md w-full outline-none focus:outline-none focus:ring-2 focus:ring-blue-500"
                    {...field}
                  />
                )}
              />
            </FormControl>
            <FormMessage />
          </FormItem>

          <FormItem>
            <FormControl>
              <Controller
                name="password"
                control={form.control}
                render={({ field }) => (
                  <input
                    type="password"
                    placeholder="Password"
                    className="bg-[#eee] border-none my-2 mx-0 text-xs py-2.5 px-4 text rounded-md w-full outline-none focus:outline-none focus:ring-2 focus:ring-blue-500"
                    {...field}
                  />
                )}
              />
            </FormControl>
            <FormMessage />
          </FormItem>

          <Link to="/forgot-password" className="text-[#333] text-xs mt-3.5 mb-2.5 my-0 self-center">Forgot your password?</Link>
          <Button type="submit" variant="default"   className='bg-[#512da8] text-[#fff] text-xs py-2.5 px-11 rounded-md border border-solid font-semibold tracking-[0.5px] uppercase mt-2.5 pointer self-center'>
            Sign in
          </Button>
          </form>
        </div>
    </Form>
  );
};

export default Login;
