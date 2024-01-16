import { Input } from "../ui/input";

export default function Page() {
    return (
        <div className="flex items-center justify-center h-screen">
            <div className="px-8 pt-6 pb-8 bg-white shadow-md rounded">
                <div className="mb-4">
                    <Input label="Username" placeholder="El padre" inputType="text" />
                </div>
                <div className="mb-4">
                    <Input label="Password" placeholder="Secret12345" inputType="password" />
                </div>
                <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline" type="button">
                    Login
                </button>
            </div>

        </div>
    )
}