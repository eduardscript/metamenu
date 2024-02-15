import { useRef } from "react";

interface ModalProps {
  title: string;
  children: React.ReactNode;
  formAction: (formData: FormData) => Promise<void>;
}

const EntityModal: React.FC<ModalProps> = ({ title, children, formAction }) => {
  const dialogRef = useRef<HTMLDialogElement>(null);

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    const formData = new FormData(event.currentTarget);
    try {
      await formAction(formData);
      dialogRef.current?.close();
    } catch (error) {
      console.error("Error:", error);
    }
  };
  return (
    <>
      <div onClick={() => dialogRef.current?.showModal()}>{children}</div>
      <dialog className="modal" ref={dialogRef}>
        <div className="modal-box">
          <h3 className="font-bold text-lg">{title}</h3>
          <form onSubmit={handleSubmit}>{children}</form>
        </div>
      </dialog>
    </>
  );
};

export default EntityModal;
