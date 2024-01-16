import Link from "next/link"

export const Navigation = () => {
    return (<nav className="bg-green-800 p-4">
        <div className="container mx-auto flex justify-between items-center">
            <div className="space-x-4">
                <Link href="/" className="text-white text-xl font-bold">
                    META MENU
                </Link>
            </div>
            <div className="space-x-2">
                <Link href="/auth" className="bg-black hover:bg-green-900 text-white font-bold py-2 px-4 rounded-full">
                    Register
                </Link>
                <Link href="/auth" className="bg-black hover:bg-green-900 text-white font-bold py-2 px-4 rounded-full">
                    Login
                </Link>
            </div>
        </div>
    </nav>)
}