import React from "react";

const Input = ({ id, type, ...rest }) => {
  return <input id={id} type={type} {...rest} />;
};

export default Input;
