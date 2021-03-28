import React from "react";
import ErrorMessage from "./ErrorMessage";

const FormInput = ({
  className,
  error,
  id,
  name,
  onChange,
  title,
  type,
  value,
}) => {
  return (
    <>
      <span className="mr-2">{title}</span>
      <input
        className={className}
        id={id}
        name={name}
        type={type}
        value={value}
        onChange={onChange}
      />
      <div className="text-danger">
        {error && <ErrorMessage error={error} />}
      </div>
    </>
  );
};

export default FormInput;
