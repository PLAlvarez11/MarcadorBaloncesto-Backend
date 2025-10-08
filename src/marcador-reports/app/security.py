from typing import List
import os
from fastapi import Depends, HTTPException, status
from fastapi.security import HTTPAuthorizationCredentials, HTTPBearer
from jose import jwt, JWTError

bearer_scheme = HTTPBearer(auto_error=True)

class JwtSettings:
    issuer: str = os.getenv("JWT__ISSUER", "MarcadorApi")
    audience: str = os.getenv("JWT__AUDIENCE", "MarcadorFrontend")
    key: str = os.getenv("JWT__KEY", "dev_key_change_me")
    algorithms: List[str] = ["HS256"]

def decode_token(token: str) -> dict:
    try:
        payload = jwt.decode(
            token,
            JwtSettings.key,
            algorithms=JwtSettings.algorithms,
            audience=JwtSettings.audience,
            options={"verify_exp": True, "verify_aud": True, "verify_iss": False},
        )
        return payload
    except JWTError as ex:
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail=f"Invalid token: {str(ex)}")

def require_admin(creds: HTTPAuthorizationCredentials = Depends(bearer_scheme)) -> dict:
    token = creds.credentials
    payload = decode_token(token)
    role = payload.get("role") or payload.get("Role")
    roles = payload.get("roles") or payload.get("Roles") or []
    accesos = payload.get("accesos") or payload.get("permissions") or []

    is_admin = False
    if isinstance(role, str):
        is_admin = role.lower() == "admin"
    if isinstance(roles, list):
        is_admin = is_admin or any(str(r).lower() == "admin" for r in roles)
    if isinstance(accesos, list):
        is_admin = is_admin or any("admin" in str(a).lower() for a in accesos)

    if not is_admin:
        raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Admin role required")
    return payload
