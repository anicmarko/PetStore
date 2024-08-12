import {
    Form,
    FormItem,
    FormControl,
    FormMessage,
  } from "@/components/ui/form";
  import { useForm, Controller } from "react-hook-form";
  import { Button } from "@/components/ui/button";

  interface RegisterProps {
    isActiveContainer: boolean;
  }
  
  const Register = ({isActiveContainer} : RegisterProps) => {
    const form = useForm({
      defaultValues: {
        name: "",
        email: "",
        password: "",
        confirmPassword: "",
      },
    });
  
    const onSubmit = (data: any) => {
      console.error(data);
    };
  
    return (
      <Form {...form}>
      <div className={`flex items-center justify-center absolute top-0 h-full transition-all duration-500 ease-in-out left-0 w-1/2 opacity-0 z-[1] ${isActiveContainer ? 'translate-x-[100%] opacity-100 z-[4]': ' '} animate-move`}>

        <form
          onSubmit={form.handleSubmit(onSubmit)}
          className="flex flex-col justify-center align-middle px-10 py-0 bg-[#fff] w-full "
        >
          <h1 className="text-3xl font-semibold text-center mb-4 tracking-wider">Create Account</h1>
  
          <FormItem>
            <FormControl>
              <Controller
                name="name"
                control={form.control}
                render={({ field }) => (
                  <input
                    type="text"
                    placeholder="Name"
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
  
          <FormItem>
            <FormControl>
              <Controller
                name="confirmPassword"
                control={form.control}
                render={({ field }) => (
                  <input
                    type="password"
                    placeholder="Confirm Password"
                    className="bg-[#eee] border-none my-2 mx-0 text-xs py-2.5 px-4 text rounded-md w-full outline-none focus:outline-none focus:ring-2 focus:ring-blue-500"
                    {...field}
                  />
                )}
              />
            </FormControl>
            <FormMessage />
          </FormItem>
  
          <Button type="submit" variant="default"  className='bg-[#512da8] text-[#fff] text-xs py-2.5 px-11 rounded-md border border-solid font-semibold tracking-[0.5px] uppercase mt-2.5 pointer self-center' >
            Sign Up
          </Button>
        </form>
        </div>
      </Form>
    );
  };
  
  export default Register;